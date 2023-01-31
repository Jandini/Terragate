namespace Terragate.Api.Services
{
    public class TerraformDeploymentRepository : ITerraformDeploymentRepository
    {

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
               .Select(d => new TerraformDeployment() { Guid = Guid.Parse(d.Name), CreatedDate = d.CreationTime, WorkingDirectory = d })
               .ToArray();

            return deployments;
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