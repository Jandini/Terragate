using System.Text.Json.Serialization;

namespace Terragate.Api.Services
{
    public class ElasticHealthVersion
    {
        [JsonPropertyName("number")]
        public string? Number { get; set; }

        [JsonPropertyName("build_flavor")]
        public string? BuildFlavor { get; set; }

        [JsonPropertyName("build_type")]
        public string? BuildType { get; set; }

        [JsonPropertyName("build_hash")]
        public string? BuildHash { get; set; }

        [JsonPropertyName("build_date")]
        public string? BuildDate { get; set; }

        [JsonPropertyName("build_snapshot")]
        public bool? BuildSnapshot { get; set; }

        [JsonPropertyName("lucene_version")]
        public string? LuceneVersion { get; set; }

        [JsonPropertyName("minimum_wire_compatibility_version")]
        public string? MinimumWireCompatibilityVersion { get; set; }

        [JsonPropertyName("minimum_index_compatibility_version")]
        public string? MinimumIndexCompatibilityVersion { get; set; }
    }
}
