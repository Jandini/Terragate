namespace Terragate.Api.Services
{
    public static class TerraformExtensions
    {
        public static IServiceCollection AddTerraform(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddScoped<ITerraformProcessService, TerraformProcessService>()
                .AddScoped<ITerraformInfrastructureRepository, TerraformInfrastructureRepository>()
                .AddSingleton<ITerraformConfigurationService, TerraformConfigurationService>();
        }


        private static void ConfigureEnvironmentVariable(string name, string value, Serilog.ILogger logger)
        {
            if (Environment.GetEnvironmentVariable(name) == null)
            {
                logger.Debug("Adding {name:l} set to {dir}", name, value);
                Environment.SetEnvironmentVariable(name, value);
            }
            else
            {
                logger.Debug("Using existing {name:l} provided to the environment", name);
            }
        }

        public static IApplicationBuilder UseTerraform(this IApplicationBuilder app)
        {
            var config = app.ApplicationServices.GetRequiredService<ITerraformConfigurationService>();
            var logger = app.ApplicationServices.GetRequiredService<Serilog.ILogger>().ForContext<Program>();

            var terra = config.GetTerraformConfig();

            if (terra != null)
            {
                if (terra.Variables != null)
                {
                    foreach (var variable in terra.Variables)
                    {
                        var value = variable.GetValue();

                        if (!string.IsNullOrEmpty(value))
                        {
                            var name = $"TF_VAR_{variable.Name}";

                            if (Environment.GetEnvironmentVariable(name) == null)
                            {
                                logger.Debug("Adding {variableName:l} from configuration", name);
                                Environment.SetEnvironmentVariable(name, value);
                            }
                            else
                            {
                                logger.Debug("Using {variableName:l} provided to the environment", name);
                            }
                        }
                    }
                }

                if (terra.UsePluginCache)
                {
                    var dir = new DirectoryInfo(terra.PluginsPath);

                    if (!dir.Exists)
                    {
                        logger.Debug("Creating {dir}", dir.FullName);
                        dir.Create();
                    }

                    ConfigureEnvironmentVariable("TF_PLUGIN_CACHE_DIR", dir.FullName, logger);
                }              
            }

            return app;
        }
    }
}
