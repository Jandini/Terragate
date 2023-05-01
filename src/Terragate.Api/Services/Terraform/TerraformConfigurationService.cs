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
    }
}
