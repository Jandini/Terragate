namespace Terragate.Api.Services
{
    public interface ITerraformProcessService
    {
        Task StartAsync(string arguments, DirectoryInfo? workingDirectory);
    }
}
