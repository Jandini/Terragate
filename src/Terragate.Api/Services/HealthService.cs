using System.Reflection;

namespace Terragate.Api.Services
{
    public class HealthService : IHealthService
    {
        public static string ServiceStatus = "Running";

        public HealthInfo GetHealthInfo()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            var info = new HealthInfo
            {
                ServiceName = assembly.GetName().Name,
                ServiceVersion = version,
                ServiceStatus = HealthService.ServiceStatus                
            };

            return info;
        }
    }
}
