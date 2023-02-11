using System.Diagnostics;
using System.Text;

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
            const string NAME = "terraform";

            _logger.LogDebug("Starting {terraform:l} {args:l} in {dir}", NAME, arguments, workingDirectory);
            
            using var process = new Process();
            StringBuilder errors = new();

            process.StartInfo.FileName = NAME;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = workingDirectory?.FullName;
            
            process.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                    _logger.LogDebug(args.Data);
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    _logger.LogError(args.Data);
                    errors.AppendLine(args.Data);
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
                {                    
                    throw errors.Length > 0 
                        ? new TerraformException(errors.ToString()) 
                        : new TerraformException($"Terraform {arguments} failed with exit code {process.ExitCode}.");
                }
            }
            else
            {
                throw new TerraformException("Terraform process failed to start.");
            }                            
        }     
    }
}