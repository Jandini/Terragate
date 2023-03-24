namespace Terragate.Api.Services
{
    public class HealthInfo
    {
        public TerragateHealthInfo? Terragate { get; set; }
        public ElasticHealthInfo? Elastic { get; set; }
    }
}
