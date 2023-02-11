namespace Terragate.Api.Services
{
    public interface ITerraformInfrastructureRepository
    {
        IEnumerable<ITerraformInfrastructure> GetInfrastructures();
        ITerraformInfrastructure GetInfrastructure(Guid id);
        void DeleteInfrastructure(Guid id);
        Task<ITerraformInfrastructure> AddInfrastructure(IFormFile[] terraformFiles);
    }
}
