namespace Terragate.Api.Models
{
    public class ElasticInfoDto
    {
        public string? Name { get; set; }
        public string? ClusterName { get; set; }
        public ElasticVersionDto? Version { get; set; }
        public string? Uri { get; set; }
        public string? Status { get; set; }
    }
}
