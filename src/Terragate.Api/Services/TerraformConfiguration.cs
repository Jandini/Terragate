using System.Text;

namespace Terragate.Api.Services
{
    public class TerraformConfiguration
    {
        public enum VariableType
        {
            Plain = 0,
            Encoded = 1,
        }

        public class TerraformVariable
        {
            public string? Name { get; set; }
            public string? Value { get; set; }
            public VariableType? Type { get; set; } = VariableType.Plain;

            public string? GetValue()
            {
                if (string.IsNullOrEmpty(Value)) 
                    return Value;

                return Type switch
                {
                    VariableType.Plain => Value,
                    VariableType.Encoded => Encoding.UTF8.GetString(Convert.FromBase64String(Value)),
                    _ => throw new NotSupportedException($"Variable type '{Type}' is not supported.")
                }; ;
            }
        }

        public class TerraformVariables : List<TerraformVariable> 
        { 

        }      

        /// <summary>
        /// Enable or disable data/plugins cache directory 
        /// </summary>
        public bool UsePluginCache { get; set; } = true;

        /// <summary>
        /// Copy all *.tf file from app folder to infrastructure.        
        /// </summary>
        public bool UseTemplates { get; set; } = true;

        /// <summary>
        /// Terraform data directory
        /// </summary>
        public string DataDir { get; set; } = "data";

        /// <summary>
        /// Templates directory path
        /// </summary>
        public string TemplatesPath { get => Path.Combine(DataDir, "templates"); }

        /// <summary>
        /// Plugins cache directory path
        /// </summary>
        public string PluginsPath { get => Path.Combine(DataDir, "plugins"); }

        /// <summary>
        /// Infrastructures home directory path
        /// </summary>
        public string InfrasPath { get => Path.Combine(DataDir, "infras"); }


        /// <summary>
        /// Variables can be provided from appsettings.json and secrets.json.
        /// The conent of secrets.json overrides appsettings.json configuration. 
        /// 
        /// secrets.json:
        /// 
        /// "Terraform:Variables": [
        ///     {
        ///       "Name": "VRA_USER",
        ///       "Value": ""
        ///     },
        ///     {
        ///     "Name": "VRA_PASS",
        ///       "Value": ""
        ///     },
        ///     {
        ///     "Name": "VRA_TENANT",
        ///       "Value": ""
        ///     },
        ///     {
        ///     "Name": "VRA_HOST",
        ///       "Value": ""
        ///     },
        ///     {
        ///     "Name": "DEPLOYMENT_CATALOG_NAME",
        ///       "Value": ""
        ///     },
        ///     {
        ///     "Name": "RESOURCE_COMPONENT_NAME",
        ///       "Value": ""
        ///     }
        /// ]        
        public TerraformVariables? Variables { get; set; } 

    }
}
