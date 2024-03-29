using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
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


        [HttpPost("files", Name = "CreateInfrastructureFromFiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(IFormFile[] files, string? variables)
        {
            ITerraformInfrastructure? infra = null;

            try
            {
                _logger.LogInformation($"Creating infrastructure from {files.Select(a => a.FileName)}");
                infra = await _repository.AddInfrastructure(files);

                using (LogContext.PushProperty("InfrastructureId", infra.Id.ToString()))
                {
                    // add variables to command line if provided
                    var values = (variables?.Split(';').Where(a => a.Trim() != string.Empty))?.ToArray() ?? Array.Empty<string>();
                    var vars = values.Length > 0 ? " -var " + string.Join(" -var ", values) : string.Empty;

                    await _terraform.StartAsync("init -no-color -input=false" + vars, infra.WorkingDirectory);
                    await _terraform.StartAsync("apply -no-color -auto-approve -input=false" + vars, infra.WorkingDirectory);

                    infra = _repository.GetInfrastructure(infra.Id);

                    if (!infra.Resources.Any())
                        throw new TerraformException("Terraform completed successfully but no resourcre waw created.");


                    // It is possible that VM is still 'TurningOn'.
                    // Make sure all the resources are in 'On' status before give away the infrastructure.
                    int retry = 0;
                    while (infra.Resources.Any(a => a.Instances.Where(i => i.Status != "On").Any()))
                    {
                        var resources = infra.Resources.SelectMany(a => a.Instances.Where(i => i.Status != "On"));

                        foreach (var resource in resources)
                        {
                            _logger.LogDebug($"Waiting for {resource.HostName} with status {resource.Status}...");
                            await Task.Delay(1000);
                        }

                        await _terraform.StartAsync("refresh -no-color -input=false", infra.WorkingDirectory);
                        infra = _repository.GetInfrastructure(infra.Id);

                        if (++retry > 10)
                            throw new TerraformException("Resources are not ready.");
                    }

                    return Ok(_mapper.Map<InfrastructureDto>(infra));
                }
            }
            catch (Exception)
            {
                if (infra != null)
                    _repository.DeleteInfrastructure(infra.Id);

                throw;
            }
        }   

        [HttpPost("url", Name = "CreateInfrastructureFromUrl")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(string url, string? variables)
        {
            return await Post(new IFormFile[] { await _repository.DownloadFile(url) }, variables);
        }
    }
}