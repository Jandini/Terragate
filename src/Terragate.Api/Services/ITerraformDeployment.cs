namespace Terragate.Api.Services
{
    public interface ITerraformDeployment
    {
        Guid Guid { get; }
        DirectoryInfo? WorkingDirectory { get; }
        ITerraformDeploymentInstance[]? Instances { get; }
    }
}