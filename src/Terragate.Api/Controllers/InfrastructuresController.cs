using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Terragate.Api.Models;
using Terragate.Api.Services;

namespace Terragate.Api.Controllers
{
    [ApiController]
    [Route("api/infrastructures")]
    public class InfrastructuresController : ControllerBase
    {
        private readonly ILogger<InfrastructuresController> _logger;
        private readonly IMapper _mapper;
        private readonly ITerraformProcessService _terraform;
        private readonly ITerraformInfrastructureRepository _repository;

        public InfrastructuresController(ILogger<InfrastructuresController> logger, IMapper mapper, ITerraformProcessService terraform, ITerraformInfrastructureRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _terraform = terraform;
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<InfrastructureDto>> Get()
        {
            var infrastructures = _repository.GetInfrastructures().Where(a => a.Resources.Any());

            var results = _mapper.Map<IEnumerable<InfrastructureDto>>(infrastructures);

            if (results.Any())
                _logger.LogDebug("{@infrastructures}", results);

            return Ok(results);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(IFormFile[] files)
        {
            var deployment = await _repository.AddInfrastructure(files);

            try
            {
                await _terraform.StartAsync("init -no-color -input=false", deployment.WorkingDirectory);
                await _terraform.StartAsync("apply -no-color -auto-approve -input=false", deployment.WorkingDirectory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(_mapper.Map<InfrastructureDto>(deployment));
        }
    }
}