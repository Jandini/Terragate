using AutoMapper;
using System.Text.Json;

namespace Terragate.Api.Services
{
    public class TerraformInfrastructureRepository : ITerraformInfrastructureRepository
    {
        const string TERRAFORM_STATE_FILE = "terraform.tfstate";
        const string TERRAFORM_STATE_LOCK_FILE = "terraform.tfstate.lock.info";
        
        private readonly ILogger<TerraformInfrastructureRepository> _logger;
        private readonly DirectoryInfo _root;
        private readonly IMapper _mapper;


        public TerraformInfrastructureRepository(ILogger<TerraformInfrastructureRepository> logger, ITerraformConfigurationService config, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;

            _root = new DirectoryInfo(config.GetTerraformConfig().InfrasPath);

            if (!_root.Exists)
            {
                _logger.LogDebug("Creating {root} directory", _root.FullName);
                _root.Create();
            }           
        }

        public IEnumerable<ITerraformInfrastructure> GetInfrastructures()
        {          
            var infrastructures = _root.GetDirectories("????????-????-????-????-????????????")               
               .Where(infraDir => new FileInfo(Path.Combine(infraDir.FullName, TERRAFORM_STATE_FILE)).Exists 
                    && !new FileInfo(Path.Combine(infraDir.FullName, TERRAFORM_STATE_LOCK_FILE)).Exists)               
               .Select(infraDir => new TerraformInfrastructure(infraDir, this) )
               .ToArray();

            return infrastructures;
        }


        private DirectoryInfo GetInfrastructureDir(Guid id)
        {           
            return new(Path.Combine(_root.FullName, id.ToString()));
        }

        public ITerraformInfrastructure GetInfrastructure(Guid id)
        {
            var dir = GetInfrastructureDir(id);

            if (!dir.Exists)
            {
                _logger.LogError("Infrastructure directory not found. {dir} ", dir.FullName);
                throw new DirectoryNotFoundException(dir.Name);
            }

            var state = new FileInfo(Path.Combine(dir.FullName, TERRAFORM_STATE_FILE));

            if (!state.Exists)
            {
                _logger.LogError("Infrastructure state file not found. {file} ", state.FullName);
                throw new FileNotFoundException(state.Name);
            }

            return new TerraformInfrastructure(dir, this); 
        }



        private TDestination Map<TSource1, TSource2, TSource3, TDestination>(TSource1 source1, TSource2 source2, TSource3 source3) =>
            _mapper.Map(source3, _mapper.Map(source2, _mapper.Map<TSource1, TDestination>(source1)));


        public IEnumerable<ITerraformInfrastructureResource> GetResources(DirectoryInfo infraDir)
        {
            var resources = new List<ITerraformInfrastructureResource>();
            var file = Path.Combine(infraDir.FullName, TERRAFORM_STATE_FILE);
           
            try
            {
                var json = File.ReadAllText(file);
                TerraformState? terraformState = JsonSerializer.Deserialize<TerraformState>(json);                

                if (terraformState != null && terraformState.Resources != null)
                {
                    foreach (var terraformResource in terraformState.Resources)
                    {
                        if (terraformResource.Instances != null)
                        {
                            foreach (var ri in terraformResource.Instances)
                            {
                                if (ri.Attributes != null)
                                {
                                    if (ri.Attributes.RequestStatus == "SUCCESSFUL")
                                    {
                                        if (ri.Attributes.ResourceConfiguration != null)
                                        {
                                            foreach (var rc in ri.Attributes.ResourceConfiguration)
                                            {
                                                if (rc.Instances != null)
                                                {
                                                    var instances = rc.Instances
                                                        .Where(i => !string.IsNullOrEmpty(i.IpAddress) && !string.IsNullOrEmpty(i.Name) && i.Properties != null)
                                                        .Select(i => Map<TerraformState.Instance, TerraformState.Attributes, TerraformState.Properties, TerraformInfrastructureResourceInstance>(i, ri.Attributes, i.Properties!));

                                                    if (instances.Any())
                                                        resources.Add(new TerraformInfrastructureResource(terraformResource.Name!, instances));
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        _logger.LogWarning("Resource instance {name} status is {status}", ri.Attributes?.Name, ri.Attributes?.RequestStatus);
                                    }
                                }
                            }
                        }
                    }
                }               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Reading instances from {file} failed.");
            }

            return resources.ToArray();
        }


        public async Task AddTemplates(DirectoryInfo templates, ITerraformInfrastructure infra)
        {           
            foreach (var file in templates.GetFiles())
            {
                _logger.LogWarning("Adding template {file}", file.Name);

                using var source = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, FileOptions.Asynchronous | FileOptions.SequentialScan);
                using var target = new FileStream(Path.Combine(infra.WorkingDirectory.FullName, file.Name), FileMode.CreateNew, FileAccess.Write, FileShare.None, 8192, FileOptions.Asynchronous | FileOptions.SequentialScan);
                await source.CopyToAsync(target);
            }
        }

        public async Task<ITerraformInfrastructure> AddInfrastructure(IFormFile[] terraformFiles)
        {
            var id = Guid.NewGuid();
            var dir = _root.CreateSubdirectory(id.ToString());
            var infrastructure = new TerraformInfrastructure(dir);         

            foreach (var file in terraformFiles)
            {
                var path = Path.Combine(infrastructure.WorkingDirectory.FullName, file.FileName);
                
                _logger.LogDebug("Creating file {path}", Path.Combine(dir.FullName, file.FileName));
                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);
            }

            return infrastructure; 
        }


        public void DeleteInfrastructure(Guid id)
        {
            var dir = GetInfrastructureDir(id);
            dir.Delete(true);
        }
    }
}