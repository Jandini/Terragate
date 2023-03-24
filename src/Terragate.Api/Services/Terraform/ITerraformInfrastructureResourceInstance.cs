namespace Terragate.Api.Services
{
    public interface ITerraformInfrastructureResourceInstance
    {
        string? HostName { get; }
        string? IpAddress { get; }
        string? CatalogItemName { get; }
        DateTime ExpiryDate { get; }
        DateTime CreatedDate { get; }
        string? Description { get; }
        string? Status { get; set; }
        int Cpu { get; set; }
        int Memory { get; set; }
        int Storage { get; set; }
    }
}