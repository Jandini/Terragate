namespace Terragate.Api.Services
{
    public interface ITerraformInfrastructure
    {
        Guid Id { get; }
        DirectoryInfo WorkingDirectory { get; }
        IEnumerable<ITerraformInfrastructureResource> Resources { get; }
    }
}