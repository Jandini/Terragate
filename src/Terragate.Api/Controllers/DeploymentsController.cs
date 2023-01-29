using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;


        public DeploymentsController(ILogger<DeploymentsController> logger, ITerraformProcessService terraform, ITerraformDeploymentRepository repository, IMapper mapper, IConfiguration config)
        {
            _logger = logger;
            _terraform = terraform;
            _repository = repository;
            _mapper = mapper;
            _config = config;          
        }

        [HttpGet(Name = "GetDeployments")]
        public ActionResult<IEnumerable<DeploymentDto>> Get()
        {
            var deployments = _repository.GetDeployments();
            var results = _mapper.Map<IEnumerable<DeploymentDto>>(deployments);

            _logger.LogDebug("{@deployments}", results);

            return Ok(results);
        }


        [HttpPost(Name = "CreateDeployment")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var deployment = await _repository.AddDeployment(file);


            var options = new TerraformProcessOptions()
            {
                Arguments = "-no-color",
                WorkingDirectory = deployment.WorkingDirectory
            };
            
            await _terraform.StartAsync(TerraformProcessCommand.Init, options);

            options.Variables = new Dictionary<string, object?>()
            {
                {  "VRA_COUNT", 1 },
                {  "VRA_USER", _config["Vra:User"] },
                {  "VRA_PASS", _config["Vra:Pass"] }
            };

            options.Arguments = "-no-color -auto-approve";

            _terraform.SetPluginCacheDirectory(new DirectoryInfo(".plugins"));

            await _terraform.StartAsync(TerraformProcessCommand.Apply, options);


            return Ok(_mapper.Map<DeploymentDto>(deployment));
        }
    }
}