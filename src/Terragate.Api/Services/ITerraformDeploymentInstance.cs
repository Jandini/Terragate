namespace Terragate.Api.Services
{
    public interface ITerraformDeploymentInstance
    {
        string HostName { get; }
        string IpAddress { get; }
    }
}