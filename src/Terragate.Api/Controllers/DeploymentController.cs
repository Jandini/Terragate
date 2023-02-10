using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Terragate.Api.Models;
using Terragate.Api.Services;

namespace Terragate.Api.Controllers
{
    [ApiController]
    [Route("api/deployment/{guid}")]
    public class DeploymentController : ControllerBase
    {
        private readonly ILogger<DeploymentController> _logger;
        private readonly IMapper _mapper;
        private readonly ITerraformProcessService _terraform;
        private readonly ITerraformDeploymentRepository _repository;

        public DeploymentController(ILogger<DeploymentController> logger, IMapper mapper, ITerraformProcessService terraform, ITerraformDeploymentRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _terraform = terraform;
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<DeploymentDto> Get(Guid guid)
        {
            try
            {
                var deployment = _repository.GetDeployment(guid);
                return Ok(_mapper.Map<DeploymentDto>(deployment));
            }
            catch (Exception e) when (e is DirectoryNotFoundException || e is FileNotFoundException)
            {                
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(Guid guid)
        {           
            try
            {
                var deployment = _repository.GetDeployment(guid);
                await _terraform.StartAsync("init -no-color -input=false", deployment.WorkingDirectory);
                await _terraform.StartAsync("refresh -no-color -input=false", deployment.WorkingDirectory);

                deployment = _repository.GetDeployment(guid);
                return Ok(_mapper.Map<DeploymentDto>(deployment));
            }
            catch (Exception e) when (e is DirectoryNotFoundException || e is FileNotFoundException)
            {                
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

    }
}