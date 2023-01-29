namespace Terragate.Api.Services
{
    public interface ITerraformProcessService
    {
        void SetLogLevel(TerraformProcessLogLevel level);
        void SetPluginCacheDirectory(DirectoryInfo dir);
        Task StartAsync(TerraformProcessCommand command, TerraformProcessOptions? options = null);
    }
}
