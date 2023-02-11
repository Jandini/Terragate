namespace Terragate.Api.Services
{
    internal class TerraformInfrastructureResource : ITerraformInfrastructureResource
    {
        public string Name {get; set;}

        public IEnumerable<ITerraformInfrastructureResourceInstance> Instances { get; private set; }

        public TerraformInfrastructureResource (string name, IEnumerable<ITerraformInfrastructureResourceInstance> instance)
        {
            Name = name;
            Instances = instance;
        }
    }
}