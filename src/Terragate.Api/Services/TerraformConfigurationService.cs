namespace Terragate.Api.Services
{
    public class TerraformConfigurationService : ITerraformConfigurationService
    {
        private readonly ILogger<TerraformConfigurationService> _logger;
        private readonly IConfiguration _config;
        private readonly TerraformConfiguration _terra;

        public TerraformConfigurationService(ILogger<TerraformConfigurationService> logger, IConfiguration config)
        {
            _logger = logger;            
            _config = config;
            _terra = GetTerraformConfig();
        }
   

        public DirectoryInfo GetTemplatesDir()
        {
           return new DirectoryInfo(_terra.TemplatesPath);
        }

        public TerraformConfiguration GetTerraformConfig()
        {
            var config = _config.GetSection("Terraform").Get<TerraformConfiguration>();
                
            if (config == null)
            {
                _logger.LogWarning("Terraform section in configuration is missing. Using default configuration.");
                return new TerraformConfiguration();
            }

            return config;
        }

        public bool UseTemplates(out DirectoryInfo templates)
        {
            templates = GetTemplatesDir();

            if (!_terra.UseTemplates)
                return false;

            if (templates.Exists)
            {
                return true;
            }
            else
            {
                _logger.LogWarning("Configuration indicates use of templates but the templates directory {dir} does not exist.", templates.FullName);
                return false;
            }
        }
    }
}
