using Microsoft.AspNetCore.Mvc;
using Terragate.Api.Models;
using Terragate.Api.Services;

namespace Terragate.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeploymentsController : ControllerBase
    {
        private readonly ILogger<DeploymentsController> _logger;
        private readonly ITerraformProcessService _terraform;

        public DeploymentsController(ILogger<DeploymentsController> logger, ITerraformProcessService terraform)
        {
            _logger = logger;
            _terraform = terraform;
        }

        [HttpGet(Name = "GetDeployments")]
        public IEnumerable<DeploymentDto> Get()
        {
            var deployments = new DirectoryInfo(".vra")
                .GetDirectories().Select(d => new DeploymentDto() { Name = d.Name, Date = DateOnly.FromDateTime(d.CreationTime) })
                .ToArray();
            
            _logger.LogInformation("{@deployments}", deployments);
            return deployments;
        }


        [HttpPost(Name = "CreateDeployment")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var deploymentDir = Path.Combine(".vra", Guid.NewGuid().ToString());
            Directory.CreateDirectory(deploymentDir);

            var filePath = Path.ChangeExtension(Path.Combine(deploymentDir, Path.GetRandomFileName()), "tf");

            using (var stream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(stream);

            await _terraform.StartAsync(TerraformProcessCommand.Init, new TerraformProcessOptions() { Arguments = "-no-color", WorkingDirectory = deploymentDir });

            return Ok(new { file.Length, filePath });
        }
    }
}