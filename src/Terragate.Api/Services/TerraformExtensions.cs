using AutoMapper;
using static Terragate.Api.Services.TerraformState;

namespace Terragate.Api.Services
{
    public static class TerraformExtensions
    {
        public static IServiceCollection AddTerraform(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection("Terraform").Get<TerraformConfiguration>() ?? new TerraformConfiguration();

            return services
                .AddScoped<ITerraformProcessService, TerraformProcessService>()
                .AddScoped<ITerraformInfrastructureRepository, TerraformInfrastructureRepository>()
                .AddSingleton(config);
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
            var config = app.ApplicationServices.GetRequiredService<TerraformConfiguration>();
            var logger = app.ApplicationServices.GetRequiredService<Serilog.ILogger>().ForContext<Program>();

            if (config != null)  
            {
                if (config.Variables != null)
                {
                    foreach (var variable in config.Variables)
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

                if (config.UsePluginCache)
                {
                    var dir = new DirectoryInfo(config.PluginsDir);

                    if (!dir.Exists)
                    {
                        logger.Debug("Creating {dir}", dir.FullName);
                        dir.Create();
                    }

                    ConfigureEnvironmentVariable("TF_PLUGIN_CACHE_DIR", dir.FullName, logger);
                }

                if (config.UseTemplates)
                {
                    var dir = new DirectoryInfo(config.TemplatesDir);

                    if (!dir.Exists)
                    {                                                
                        try
                        {
                            logger.Debug("Creating templates in {templates}", dir.FullName);

                            dir.Create();

                            logger.Warning("Templates are created only once");
                            logger.Warning("You can add, remove or alter the templates");
                            logger.Warning("The template files will be added to every new infrastructure");
                            logger.Warning("Ensure the template files have unique name");

                            foreach (var file in new DirectoryInfo("Templates").GetFiles())
                            {
                                logger.Debug("Copying {file}", file.Name);
                                File.Copy(file.FullName, Path.Combine(dir.FullName, file.Name));
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex, "Copy templates failed");
                        }
                    }
                }
            }

            return app;
        }
    }
}
