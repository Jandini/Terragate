
namespace Terragate.Api.Models
{
    public class InfrastructureResourceInstanceDto
    {
        public string? HostName { get; set; }
        public string? IpAddress { get; set; }
        public string? CatalogItemName { get; set; }
        public string? Description { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Status { get; set; }
        public int Cpu { get; set; }        
        public int Memory { get; set; }
        public int Storage { get; set; }
    }
}