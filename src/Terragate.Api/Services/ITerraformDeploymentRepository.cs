namespace Terragate.Api.Services
{
    public interface ITerraformDeploymentRepository
    {
        IEnumerable<ITerraformDeployment> GetDeployments();

        Task<ITerraformDeployment> AddDeployment(IFormFile file);
    }
}
