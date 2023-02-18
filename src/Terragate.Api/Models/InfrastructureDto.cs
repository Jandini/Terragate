
namespace Terragate.Api.Models
{
    public class InfrastructureDto
    {
        public Guid? Id { get; set; }
        public IEnumerable<InfrastructureResrouceDto>? Resources { get; set; }
    }
}