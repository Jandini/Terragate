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
        public ActionResult<TerragateHealthInfoDto> GetHealthInfo()
        {
            try
            {
                _logger.LogDebug("Getting health info");
                var healthInfo = _healthService.GetHealthInfo();
                return Ok(_mapper.Map<TerragateHealthInfoDto>(healthInfo));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}