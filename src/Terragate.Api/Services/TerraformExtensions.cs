namespace Terragate.Api.Services
{
    public static class TerraformExtensions
    {
        public static IServiceCollection AddTerraform(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection("Terraform").Get<TerraformConfiguration>() ?? new TerraformConfiguration();

            return services
                .AddScoped<ITerraformProcessService, TerraformProcessService>()
                .AddScoped<ITerraformDeploymentRepository, TerraformDeploymentRepository>()
                .AddSingleton(config);
        }

        public static IApplicationBuilder UseTerraform(this IApplicationBuilder app)
        {
            var config = app.ApplicationServices.GetRequiredService<TerraformConfiguration>();
            var logger = app.ApplicationServices.GetRequiredService<Serilog.ILogger>()
                .ForContext<Program>();

            if (config != null)  
            {
                if (config.Variables != null)
                {
                    foreach (var variable in config.Variables)
                    {
                        var name = $"TF_VAR_{variable.Name}";

                        if (Environment.GetEnvironmentVariable(name) == null)
                        {
                            logger.Debug("Adding {variableName:l} from configuration", name);
                            Environment.SetEnvironmentVariable(name, variable.Value);
                        }
                        else
                        {
                            logger.Debug("Environment variable {variableName:l} already exists", name);
                        }
                    }
                }

                if (config.Directories?.Plugins != null)
                {
                    var dir = new DirectoryInfo(config.Directories.Plugins);

                    if (!dir.Exists)
                    {
                        logger.Debug("Creating {dir}", dir.FullName);
                        dir.Create();
                    }

                    logger.Debug("Adding TF_PLUGIN_CACHE_DIR set to {dir}", dir.FullName);
                    Environment.SetEnvironmentVariable("TF_PLUGIN_CACHE_DIR", dir.FullName);
                }

                if (config.LogLevel != null)
                {
                    logger.Debug("Adding TF_LOG set to {level}", config.LogLevel);
                    Environment.SetEnvironmentVariable("TF_LOG", config.LogLevel);
                }
            }

            return app;
        }
    }
}
