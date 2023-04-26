using System.Diagnostics;
using System.Text;

namespace Terragate.Api.Services
{
    public class TerraformProcessService : ITerraformProcessService
    {
        private readonly ILogger<TerraformProcessService> _logger;
        private DateTime _timestamp;
        private int _timebuffer; 

        public TerraformProcessService(ILogger<TerraformProcessService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _timebuffer = configuration.GetValue("LOG_BUFFER_MS", 1000);
        }


        private void LogData(string? data, StringBuilder buffer, Action<string> log)
        {
            if (!string.IsNullOrEmpty(data))
            {
                if (buffer.Length > 0 && DateTime.UtcNow.Subtract(_timestamp).TotalMilliseconds > _timebuffer)
                {
                    log.Invoke(buffer.ToString());
                    buffer.Clear();
                    _timestamp = DateTime.UtcNow;
                }
                
                buffer.AppendLine(data);
            }
        }

        public async Task StartAsync(string arguments, DirectoryInfo? workingDirectory)
        {
            const string NAME = "terraform";

            _logger.LogDebug("Starting {terraform:l} {args:l} in {dir}", NAME, arguments, workingDirectory);

            using var process = new Process();

            process.StartInfo.FileName = NAME;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = workingDirectory?.FullName;

            _timestamp = DateTime.UtcNow;
            var errors = new StringBuilder();
            var outputBuffer = new StringBuilder();
            var errorsBuffer = new StringBuilder();

            process.OutputDataReceived += (sender, args) => LogData(args.Data, outputBuffer, (lines) => _logger.LogDebug(lines));
            process.ErrorDataReceived += (sender, args) => LogData(args.Data, errorsBuffer, (lines) => 
            { 
                _logger.LogError(lines); 
                errors.AppendLine(lines); 
            });

            if (process.Start())
            {
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await process.WaitForExitAsync();
                process.CancelOutputRead();
                process.CancelErrorRead();

                if (outputBuffer.Length > 0)
                    _logger.LogDebug(outputBuffer.ToString());

                if (errorsBuffer.Length > 0)
                {
                    var buffer = errorsBuffer.ToString();
                    errors.AppendLine(buffer);
                    _logger.LogError(buffer);
                }


                _logger.LogDebug("Terraform process finished with exit code {ExitCode}", process.ExitCode);

                if (process.ExitCode != 0)
                {
                    var message = errors.ToString();

                    if (!string.IsNullOrEmpty(message))
                    {
                        if (message.Contains("The resource cannot be found"))
                            throw new TerraformInfrastructureNotFoundException(message);
                        else
                            throw new TerraformException(message);
                    }
                    else
                    {
                        new TerraformException($"Terraform {arguments} failed with exit code {process.ExitCode}.");
                    }
                }
            }
            else
            {
                throw new TerraformException("Terraform process failed to start.");
            }
        }
    }
}
