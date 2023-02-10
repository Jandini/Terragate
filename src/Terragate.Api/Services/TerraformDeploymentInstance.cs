namespace Terragate.Api.Services
{
    public class TerraformDeploymentInstance : ITerraformDeploymentInstance
    {
        public string? HostName { get; internal set; }
        public string? IpAddress { get; internal set; }
        public string? CatalogItemName { get; internal set; }
        public DateTime ExpiryDate { get; internal set; }
        public DateTime CreatedDate { get; internal set; }
    }
}
