using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Terragate.Api.Models;
using Terragate.Api.Services;

namespace Terragate.Api.Controllers
{
    [ApiController]
    [Route("api/infrastructure/{id}")]
    public class InfrastructureController : ControllerBase
    {
        private readonly ILogger<InfrastructureController> _logger;
        private readonly IMapper _mapper;
        private readonly ITerraformProcessService _terraform;
        private readonly ITerraformInfrastructureRepository _repository;

        public InfrastructureController(ILogger<InfrastructureController> logger, IMapper mapper, ITerraformProcessService terraform, ITerraformInfrastructureRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _terraform = terraform;
            _repository = repository;
        }

        /// <summary>
        /// Get deployment by id
        /// </summary>
        /// <param name="id">The id of deployment to get</param>
        /// <returns>Requested deployment</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<InfrastructureDto> Get(Guid id)
        {
            try
            {
                var deployment = _repository.GetInfrastructure(id);
                return Ok(_mapper.Map<InfrastructureDto>(deployment));
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


        /// <summary>
        /// Refresh the deployment by running terragate init
        /// </summary>
        /// <param name="id">The id of the deployment to refresh</param>
        /// <returns>Refreshed deployment</returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Refresh(Guid id)
        {           
            try
            {
                var deployment = _repository.GetInfrastructure(id);
                await _terraform.StartAsync("init -no-color -input=false", deployment.WorkingDirectory);
                await _terraform.StartAsync("refresh -no-color -input=false", deployment.WorkingDirectory);

                deployment = _repository.GetInfrastructure(id);
                return Ok(_mapper.Map<InfrastructureDto>(deployment));
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



        /// <summary>
        /// Destroy the deployment 
        /// </summary>
        /// <param name="id">The id of the deployment to destroy</param>
        /// <returns>Refreshed deployment</returns>
        [HttpDelete()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Destroy(Guid id)
        {
            try
            {
                var deployment = _repository.GetInfrastructure(id);
                await _terraform.StartAsync("init -no-color -input=false", deployment.WorkingDirectory);
                await _terraform.StartAsync("destroy -input=false -auto-approve", deployment.WorkingDirectory);

                _repository.DeleteInfrastructure(id);
                
                return NoContent();
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