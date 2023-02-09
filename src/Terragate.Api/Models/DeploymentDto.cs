using Terragate.Api.Services;

namespace Terragate.Api.Models
{
    public class DeploymentDto
    {
        public Guid Guid { get; set; }
        public DateTime CreatedDate { get; set; }
        public ITerraformDeploymentInstance[]? Instances { get; set; }
    }
}