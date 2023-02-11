namespace Terragate.Api.Services
{
    public interface ITerraformInfrastructureResource
    {
        string Name { get; }
        IEnumerable<ITerraformInfrastructureResourceInstance> Instances { get; }
    }
}
