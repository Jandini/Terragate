using System.Diagnostics;
using System.Text;

namespace Terragate.Api.Services
{
    public class TerraformProcessService : ITerraformProcessService
    {

        private ILogger<TerraformProcessService> _logger { get; }
        private List<string> _output;        


        public TerraformProcessService(ILogger<TerraformProcessService> logger)
        {
            _logger = logger;
            _output = new List<string>(); 
        }


        public async Task StartAsync(TerraformProcessCommand command, TerraformProcessOptions? options)
        {
            var args = new StringBuilder(command.ToString().ToLower());

            if (!string.IsNullOrEmpty(options?.Arguments))
            {
                args.Append(' ');
                args.Append(options.Arguments);
            }

            if (options?.Variables != null)
            {
                args.Append(' ');
                args.Append(string.Join(" ", options.Variables.Select(a => "-var " + a.Key + "=" + a.Value)));
            }

            var name = "terraform";
            var workingDir = options?.WorkingDirectory ?? Directory.GetCurrentDirectory();

            _logger.LogDebug("Starting {terraform} {args} in {dir}", name, args, workingDir);
            
            using var process = new Process();

            process.StartInfo.FileName = name;
            process.StartInfo.Arguments = args.ToString();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = workingDir;
            
            process.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    _output.Add(args.Data);
                    _logger.LogDebug(args.Data);
                }
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    _output.Add(args.Data);
                    _logger.LogError(args.Data);
                }
            };
            

            if (process.Start())
            {
                process.BeginOutputReadLine();
                await process.WaitForExitAsync();                
                process.CancelOutputRead();

                _logger.LogDebug("Terraform process exited with {ExitCode}", process.ExitCode);

                if (process.ExitCode != 0)
                    throw new Exception($"Terraform {command.ToString().ToLower()} failed.");
            }
            else
            {
                throw new Exception("Terraform process failed to start.");
            }                            
        }

        public void SetLogLevel(TerraformProcessLogLevel level)
        {
            _logger.LogDebug("Setting TF_LOG environment varialbe to {level}", level);
            Environment.SetEnvironmentVariable("TF_LOG", level.ToString());
        }
    }
}