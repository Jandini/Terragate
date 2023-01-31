namespace Terragate.Api.Services
{
    public interface ITerraformDeployment
    {
        Guid Guid { get; }
        DateTime CreatedDate { get; }
        DirectoryInfo? WorkingDirectory { get; }
        ITerraformDeploymentInstance[]? Instances { get; }
    }
}