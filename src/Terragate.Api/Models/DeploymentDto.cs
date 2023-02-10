
namespace Terragate.Api.Models
{
    public class DeploymentDto
    {
        public Guid Guid { get; set; }
        public DeploymentInstanceDto[]? Instances { get; set; }
    }
}