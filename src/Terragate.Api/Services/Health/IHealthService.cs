namespace Terragate.Api.Services
{
    public interface IHealthService
    {
        Task<HealthInfo> GetHealthInfoAsync();
    }
}