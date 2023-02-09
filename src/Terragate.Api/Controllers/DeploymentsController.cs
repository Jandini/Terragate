using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Terragate.Api.Models;
using Terragate.Api.Services;

namespace Terragate.Api.Controllers
{
    [ApiController]
    [Route("api/deployments")]
    public class DeploymentsController : ControllerBase
    {
        private readonly ILogger<DeploymentsController> _logger;
        private readonly IMapper _mapper;
        private readonly ITerraformProcessService _terraform;
        private readonly ITerraformDeploymentRepository _repository;

        public DeploymentsController(ILogger<DeploymentsController> logger, IMapper mapper, ITerraformProcessService terraform, ITerraformDeploymentRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _terraform = terraform;
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DeploymentDto>> Get()
        {
            var deployments = _repository.GetDeployments();
            var results = _mapper.Map<IEnumerable<DeploymentDto>>(deployments);

            if (results.Any())
                _logger.LogDebug("{@deployments}", results);

            return Ok(results);
        }


        [HttpPost]
        public async Task<IActionResult> Post(IFormFile[] files)
        {
            var deployment = await _repository.AddDeployment(files);

            try
            {

                await _terraform.StartAsync("init -no-color -input=false", deployment.WorkingDirectory);
                await _terraform.StartAsync("apply -no-color -auto-approve -input=false", deployment.WorkingDirectory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(_mapper.Map<DeploymentDto>(deployment));
        }
    }
}