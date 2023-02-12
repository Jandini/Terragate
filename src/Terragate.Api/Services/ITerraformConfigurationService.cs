namespace Terragate.Api.Services
{
    public interface ITerraformConfigurationService
    {
        TerraformConfiguration GetTerraformConfig();
        DirectoryInfo GetTemplatesDir();
        bool UseTemplates(out DirectoryInfo templates);
    }
}