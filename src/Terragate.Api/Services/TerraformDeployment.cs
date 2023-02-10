namespace Terragate.Api.Services
{
    public class TerraformDeployment : ITerraformDeployment
    {
        public Guid Guid { get; set; }
        public ITerraformDeploymentInstance[]? Instances { get; set; }
        public DirectoryInfo? WorkingDirectory { get; set; }
    }
}
