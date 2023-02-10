namespace Terragate.Api.Services
{
    public interface ITerraformDeploymentRepository
    {
        IEnumerable<ITerraformDeployment> GetDeployments();
        ITerraformDeployment GetDeployment(Guid guid);
        Task<ITerraformDeployment> AddDeployment(IFormFile[] terraformFiles);
    }
}
