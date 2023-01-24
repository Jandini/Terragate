using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Terragate.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
            
            _logger.LogInformation("{@result}", result);
            return result;
        }


        [HttpPost(Name = "UploadFile")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var path = Path.Combine(".vra", Guid.NewGuid().ToString());
            Directory.CreateDirectory(path);

            path = Path.ChangeExtension(Path.Combine(path, Path.GetRandomFileName()), "tf");

            using (var stream = new FileStream(path, FileMode.Create))
            await file.CopyToAsync(stream);

            return Ok(new { file.Length, path });
        }
    }
}