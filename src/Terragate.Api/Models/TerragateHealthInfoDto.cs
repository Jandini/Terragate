namespace Terragate.Api.Models
{
    public class TerragateHealthInfoDto
    {
        public string? Name { get; set; }
        public string? Version { get; set; }
        public string? Status { get; set; }
        public ElasticHealthInfoDto? Elastic { get; set; }
    }
}
