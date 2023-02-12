using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Terragate.Api.Models;

namespace Terragate.Api.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<HealthDto> Get()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "Unknown";
                var health = new HealthDto() { TerragateVersion = version };

                _logger.LogDebug("{@health}", health);

                return Ok(health);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}