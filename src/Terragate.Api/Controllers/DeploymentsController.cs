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
        private readonly ITerraformDeploymentRepository _repository;

        public DeploymentsController(ILogger<DeploymentsController> logger, ITerraformProcessService terraform, ITerraformDeploymentRepository repository)
        {
            _logger = logger;
            _terraform = terraform;
            _repository = repository;
        }

        [HttpGet(Name = "GetDeployments")]
        public IEnumerable<DeploymentDto> Get()
        {
            var deployments = _repository.GetDeployments()
                .Select(d => new DeploymentDto() { Guid = d.Guid, CreatedDate = d.CreatedDate })
                .ToArray();
            
            _logger.LogInformation("{@deployments}", deployments);
            return deployments;
        }


        [HttpPost(Name = "CreateDeployment")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var deployment = await _repository.AddDeployment(file);
            
            await _terraform.StartAsync(
                TerraformProcessCommand.Init, 
                new TerraformProcessOptions() { 
                    Arguments = "-no-color", 
                    WorkingDirectory = deployment.WorkingDirectory 
                });

            return Ok(new DeploymentDto() { Guid = deployment.Guid, CreatedDate = deployment.CreatedDate });
        }
    }
}