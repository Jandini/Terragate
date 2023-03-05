using System.Reflection;

namespace Terragate.Api.Services
{
    public class HealthService : IHealthService
    {
        private readonly IConfiguration _configuration;

        public HealthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public HealthInfo GetHealthInfo()
        {

            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            var name = assembly.GetName().Name;

            var info = new HealthInfo
            {
                ServiceName = _configuration.GetValue<string>("Service:Name") ?? name,
                ServiceVersion = _configuration.GetValue<string>("Service:Version") ?? version,
                ServiceStatus = !string.IsNullOrEmpty(_configuration.GetValue<string>("Service:Version")) ? $"Running on {name} {version}" : "Running",
            };

            return info;
        }
    }
}
