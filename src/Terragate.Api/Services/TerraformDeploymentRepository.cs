using System.Text.Json;
using Terragate.Api.Models;

namespace Terragate.Api.Services
{
    public class TerraformDeploymentRepository : ITerraformDeploymentRepository
    {
        const string TERRAFORM_STATE_FILE = "terraform.tfstate";


        private readonly ILogger<TerraformDeploymentRepository> _logger;
        private readonly DirectoryInfo _root;
        
        public TerraformDeploymentRepository(ILogger<TerraformDeploymentRepository> logger, TerraformConfiguration configuration)
        {
            _logger = logger;
            _root = new DirectoryInfo(configuration.Directories?.Deployments ?? ".deployments");

            if (!_root.Exists)
            {
                _logger.LogDebug("Creating {root} directory", _root.FullName);
                _root.Create();
            }
        }

        public IEnumerable<ITerraformDeployment> GetDeployments()
        {          
            var deployments = _root.GetDirectories("????????-????-????-????-????????????")
               .Where(d => new FileInfo(Path.Combine(d.FullName, TERRAFORM_STATE_FILE)).Exists)
               .Select(d => new TerraformDeployment() { Guid = Guid.Parse(d.Name), CreatedDate = d.CreationTime, WorkingDirectory = d, Instances = GetInstances(d) })
               .ToArray();                 

            return deployments;
        }

        private ITerraformDeploymentInstance[] GetInstances(DirectoryInfo deploymentDir)
        {
            var instances = new List<ITerraformDeploymentInstance>();
            var file = Path.Combine(deploymentDir.FullName, TERRAFORM_STATE_FILE);

            try
            {                
                var json = File.ReadAllText(file);
                TerraformState? terraformState = JsonSerializer.Deserialize<TerraformState>(json);                

                if (terraformState != null && terraformState.Resources != null)
                {
                    foreach (var resource in terraformState.Resources)
                    {
                        if (resource.Instances != null)
                            foreach (var ri in resource.Instances)
                            {
                                if (ri.Attributes != null)
                                    if (ri.Attributes.RequestStatus == "SUCCESSFUL")
                                    {                                   
                                        if (ri.Attributes.ResourceConfiguration != null)
                                        foreach (var rc in ri.Attributes.ResourceConfiguration)
                                        {
                                            if (rc.Instances != null)
                                            foreach (var instance in rc.Instances)
                                            {
                                                if (instance.Name != null && instance.IpAddress != null)
                                                    instances.Add(new TerraformDeploymentInstance(instance.Name, instance.IpAddress));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        _logger.LogWarning("Resource instance {name} status is {status}", ri.Attributes.Name, ri.Attributes.RequestStatus);
                                    }
                                }
                        }
                }               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Reading instances from {file} failed.");
            }


            return instances.ToArray();
        }

        public async Task<ITerraformDeployment> AddDeployment(IFormFile[] terraformFiles)
        {
            var guid = Guid.NewGuid();
            
            var deployment = new TerraformDeployment()
            {
                Guid = guid,
                WorkingDirectory = _root.CreateSubdirectory(guid.ToString()),
                CreatedDate = DateTime.UtcNow,
            };

            foreach (var file in terraformFiles)
            {
                var path = Path.Combine(deployment.WorkingDirectory.FullName, file.FileName);
                
                _logger.LogDebug("Creating file {path}", Path.Combine(_root.Name, file.FileName));
                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);
            }

            return deployment; 

        }
    }
}