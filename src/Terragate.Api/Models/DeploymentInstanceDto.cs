namespace Terragate.Api.Models
{
    public class DeploymentInstanceDto
    {
        public string? HostName { get; set; }
        public string? IpAddress { get; set; }
        public string? CatalogItemName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}