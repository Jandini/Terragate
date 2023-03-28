using System.Reflection;

namespace Terragate.Api.Services
{
    public class HealthService : IHealthService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _elasticClient;
        private readonly ILogger<HealthService> _logger;

        public HealthService(IConfiguration configuration, ILogger<HealthService> logger, HttpClient elasticClient)
        {
            _configuration = configuration;
            _logger = logger;
            _elasticClient = elasticClient;
            _elasticClient.BaseAddress = configuration.GetValue<Uri>("ELASTICSEARCH_URI");
        }

        public async Task<HealthInfo> GetHealthInfoAsync()
        {
            _logger.LogDebug("Getting Terragate info");

            var appName = Assembly.GetExecutingAssembly().GetName().Name;
            var appVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            var info = new HealthInfo
            {
                Terragate = new TerragateInfo()
                {
                    Name = _configuration.GetValue("APPLICATION_NAME", appName),
                    Version = _configuration.GetValue("APPLICATION_VERSION", appVersion),
                    Status = $"Running on {appName} {appVersion}",
                    
                },

                Elastic = await GetElasticDetails()
            };

            return await Task.FromResult(info);
        }


        private async Task<ElasticInfo> GetElasticDetails()
        {
            _logger.LogDebug("Getting Elasticsearch info");

            ElasticInfo? info;

            try
            {
                var response = await _elasticClient.GetAsync("/");
                response.EnsureSuccessStatusCode();
                info = await response.Content.ReadFromJsonAsync<ElasticInfo>();

                if (info == null)
                    throw new Exception("Respose is empty");

                info.Status = info.Tagline;
            }
            catch (Exception ex)
            {
                info = new ElasticInfo()
                {
                    Status = ex.Message
                };
            }

            return await Task.FromResult(info);
        }
    }
}
