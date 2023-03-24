﻿namespace Terragate.Api.Models
{
    public class ElasticHealthInfoDto
    {
        public string? Name { get; set; }
        public string? ClusterName { get; set; }
        public string? ClusterUuid { get; set; }
        public ElasticHealthVersionDto? Version { get; set; }
        public string? Uri { get; set; }

    }
}
