namespace Terragate.Api.Models
{
    public class InfrastructureResrouceDto
    {
        public string? Name { get; set; }
        public IEnumerable<InfrastructureResourceInstanceDto>? Instances { get; set; }
    }
}
