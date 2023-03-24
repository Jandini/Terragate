using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Terragate.Api.Models;
using Terragate.Api.Services;

namespace Terragate.Api.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;
        private readonly IHealthService _healthService;
        private readonly IMapper _mapper;

        public HealthController(ILogger<HealthController> logger, IHealthService healthService, IMapper mapper)
        {
            _logger = logger;
            _healthService = healthService;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetHealthInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HealthInfoDto>> GetHealthInfoAsync()
        {
            _logger.LogInformation("Getting health info");
            var healthInfo = await _healthService.GetHealthInfoAsync();
            _logger.LogInformation("{@TerragateHealth}", healthInfo);

            return Ok(_mapper.Map<HealthInfoDto>(healthInfo));
        }             
    }
}