using System.Text.Json.Serialization;

namespace Terragate.Api.Services
{
    public class ElasticHealthInfo
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("cluster_name")]
        public string? ClusterName { get; set; }

        [JsonPropertyName("cluster_uuid")]
        public string? ClusterUuid { get; set; }

        [JsonPropertyName("version")]
        public ElasticHealthVersion? Version { get; set; }

        [JsonPropertyName("tagline")]
        public string? Tagline { get; set; }
        public string? Uri { get; set; }
        public string? Status { get; set; }


    }
}
