namespace Terragate.Api.Services
{
    public class TerraformInfrastructure : ITerraformInfrastructure
    {
        public Guid Id { get; set; }
        public DirectoryInfo WorkingDirectory { get; set; }
        public IEnumerable<ITerraformInfrastructureResource> Resources { get; set; }

        public TerraformInfrastructure(DirectoryInfo infraDir, TerraformInfrastructureRepository repository)
        {
            Id = Guid.Parse(infraDir.Name);
            WorkingDirectory = infraDir;
            Resources = repository.GetResources(infraDir);
        }

        public TerraformInfrastructure(DirectoryInfo infraDir)
        {
            Id = Guid.Parse(infraDir.Name);
            WorkingDirectory = infraDir;
            Resources = new List<ITerraformInfrastructureResource>();
        }

    }
}
