namespace Terragate.Api.Services
{
    public class HealthInfo
    {
        public TerragateInfo? Terragate { get; set; }
        public ElasticInfo? Elastic { get; set; }
    }
}
