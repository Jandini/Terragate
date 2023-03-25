namespace Terragate.Api.Models
{
    public class ElasticHealthInfoDto
    {
        public string? Name { get; set; }
        public string? ClusterName { get; set; }
        public ElasticHealthVersionDto? Version { get; set; }
        public string? Uri { get; set; }
        public string? Status { get; set; }
    }
}
