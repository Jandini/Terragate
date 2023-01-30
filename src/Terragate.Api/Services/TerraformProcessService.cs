using System.Diagnostics;

namespace Terragate.Api.Services
{
    public class TerraformProcessService : ITerraformProcessService
    {
        private readonly ILogger<TerraformProcessService> _logger;
        private readonly TerraformConfiguration _config;

        public TerraformProcessService(ILogger<TerraformProcessService> logger, TerraformConfiguration config)
        {
            _logger = logger;
            _config = config;
        }


        public async Task StartAsync(string arguments, DirectoryInfo? workingDirectory)
        {
            var name = "terraform";

            _logger.LogDebug("Starting {terraform} {args} in {dir}", name, arguments, workingDirectory);
            
            using var process = new Process();

            process.StartInfo.FileName = name;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = workingDirectory?.FullName;
            
            process.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    _logger.LogDebug(args.Data);
                }
            };

            var level = string.IsNullOrEmpty(_config.LogLevel)
                ? LogLevel.Error
                : LogLevel.Debug;

            process.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    _logger.Log(level, args.Data);
                }
            };
            

            if (process.Start())
            {
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await process.WaitForExitAsync();                
                process.CancelOutputRead();
                process.CancelErrorRead();

                _logger.LogDebug("Terraform process finished with exit code {ExitCode}", process.ExitCode);

                if (process.ExitCode != 0)
                    throw new Exception($"Terraform {arguments} failed with exit code {process.ExitCode}.");
            }
            else
            {
                throw new Exception("Terraform process failed to start.");
            }                            
        }     
    }
}