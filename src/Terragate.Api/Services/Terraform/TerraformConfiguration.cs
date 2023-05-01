namespace Terragate.Api.Services
{
    public class TerraformConfiguration
    {
        /// <summary>
        /// Enable or disable data/plugins cache directory 
        /// </summary>
        public bool UsePluginCache { get; set; } = true;

        /// <summary>
        /// Terraform data directory
        /// </summary>
        public string DataDir { get; set; } = "data";

        /// <summary>
        /// Plugins cache directory path
        /// </summary>
        public string PluginsPath { get => Path.Combine(DataDir, "plugins"); }

        /// <summary>
        /// Infrastructures home directory path
        /// </summary>
        public string InfrasPath { get => Path.Combine(DataDir, "infras"); }
    }
}
