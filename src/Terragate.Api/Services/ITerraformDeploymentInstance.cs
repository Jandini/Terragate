namespace Terragate.Api.Services
{
    public interface ITerraformDeploymentInstance
    {
        string? HostName { get; }
        string? IpAddress { get; }
        string? CatalogItemName { get; }
        DateTime ExpiryDate { get; }
        DateTime CreatedDate { get; }

    }
}