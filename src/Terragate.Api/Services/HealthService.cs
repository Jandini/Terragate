using System.Reflection;

namespace Terragate.Api.Services
{
    public class HealthService : IHealthService
    {
        public HealthInfo GetHealthInfo()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            var info = new HealthInfo
            {
                ServiceName = assembly.GetName().Name,
                ServiceVersion = version
            };

            return info;
        }
    }
}
