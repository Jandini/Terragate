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
        private readonly ITerraformConfigurationService _config;


        public InfrastructuresController(ILogger<InfrastructuresController> logger, IMapper mapper, ITerraformProcessService terraform, ITerraformInfrastructureRepository repository, ITerraformConfigurationService config)
        {
            _logger = logger;
            _mapper = mapper;
            _terraform = terraform;
            _repository = repository;
            _config = config;
        }

        [HttpGet(Name = "GetInfrastructures")]
        public ActionResult<IEnumerable<InfrastructureDto>> Get()
        {
            _logger.LogInformation("Get infrastructures");
            var infrastructures = _repository.GetInfrastructures().Where(a => a.Resources.Any());
            var results = _mapper.Map<IEnumerable<InfrastructureDto>>(infrastructures);

            if (results.Any())
                _logger.LogDebug("{@infras}", results);

            return Ok(results);
        }


        [HttpPost(Name = "CreateInfrastructure")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(IFormFile[] files, string? variables)
        {
            ITerraformInfrastructure? infra = null;

            try
            {
                _logger.LogInformation("Create infrastructure from {terraformFiles}", files.Select(a => a.FileName));
                infra = await _repository.AddInfrastructure(files);

                // set id for serilog dynamic enricher
                AppContext.SetData("InfrastructureId", infra.Id);

                if (_config.UseTemplates(out var templates))
                    await _repository.AddTemplates(templates, infra);

                // add variables to command line if provided
                var values = (variables?.Split(';').Where(a => a.Trim() != string.Empty))?.ToArray() ?? Array.Empty<string>();
                var vars = values.Length > 0 ? " -var " + string.Join(" -var ", values) : string.Empty;

                await _terraform.StartAsync("init -no-color -input=false" + vars, infra.WorkingDirectory);
                await _terraform.StartAsync("apply -no-color -auto-approve -input=false" + vars, infra.WorkingDirectory);
                
                infra = _repository.GetInfrastructure(infra.Id);

                if (!infra.Resources.Any())                                    
                    throw new TerraformException("Terraform completed successfully but no resourcre waw created.");
                
                return Ok(_mapper.Map<InfrastructureDto>(infra));
            }
            catch (Exception)
            {
                if (infra != null)
                    _repository.DeleteInfrastructure(infra.Id);

                throw;
            }            
        }
    }
}