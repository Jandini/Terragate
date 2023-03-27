namespace Terragate.Api.Models
{
    public class HealthInfoDto
    {
        public TerragateInfoDto? Terragate { get; set; }
        public ElasticInfoDto? Elastic { get; set; }
    }
}
