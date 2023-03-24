using System.Reflection;
using Terragate.Api.Controllers;

namespace Terragate.Api.Services
{
    public class HealthService : IHealthService
    {
        private readonly AppSettings _settings;
        private readonly HttpClient _elasticClient;
        private readonly ILogger<HealthService> _logger;

        public HealthService(AppSettings settings, HttpClient elasticClient, ILogger<HealthService> logger)
        {
            _settings = settings;

            _elasticClient = elasticClient;
            _elasticClient.BaseAddress = settings.ElasticsearchUri;
            _logger = logger;
        }

        public async Task<HealthInfo> GetHealthInfoAsync()
        {

            _logger.LogInformation("Getting elasticsearch health info");

            ElasticHealthInfo? elasticHealthInfo;

            try
            {
                var response = await _elasticClient.GetAsync("/");
                response.EnsureSuccessStatusCode();
                elasticHealthInfo = await response.Content.ReadFromJsonAsync<ElasticHealthInfo>();
                elasticHealthInfo!.Uri = _settings.ElasticsearchUri?.ToString();
            }
            catch (Exception ex)
            {
                elasticHealthInfo = new ElasticHealthInfo() { Uri = ex.Message };
            }


            _logger.LogInformation("Getting terragate health info");

            var assembly = Assembly.GetExecutingAssembly();
            var appVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            var appName = assembly.GetName().Name;

            var healthInfo = new HealthInfo()
            {
                Terragate = new TerragateHealthInfo()
                {
                    Name = _settings.ApplicationName ?? appName,
                    Version = _settings.ApplicationVersion ?? appVersion,
                    Status = !string.IsNullOrEmpty(_settings.ApplicationVersion) ? $"Running on {appName} {appVersion}" : "Running",
                },

                Elastic = elasticHealthInfo                
            };

            return await Task.FromResult(healthInfo);
        }
    }
}
