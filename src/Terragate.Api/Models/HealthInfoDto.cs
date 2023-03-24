using Terragate.Api.Services;

namespace Terragate.Api.Models
{
    public class HealthInfoDto
    {
        public TerragateHealthInfoDto? Terragate { get; set; }
        public ElasticHealthInfoDto? Elastic { get; set; }

    }
}
