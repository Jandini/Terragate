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
            var appVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            var appName = assembly.GetName().Name;

            var info = new HealthInfo
            {
                ServiceName = _configuration.GetValue("App:Name", appName),
                ServiceVersion = _configuration.GetValue("App:Version", appVersion),
                ServiceStatus = !string.IsNullOrEmpty(_configuration.GetValue<string>("App:Version")) ? $"Running on {appName} {appVersion}" : "Running",
            };

            return info;
        }
    }
}
