namespace Terragate.Api.Services
{
    public interface ITerraformProcessService
    {
        void SetLogLevel(TerraformProcessLogLevel level);
        Task StartAsync(TerraformProcessCommand command, TerraformProcessOptions? options = null);
    }
}
