namespace Terragate.Api.Services
{
    public class TerraformConfiguration
    {
        public class TerraformDirectories
        {
            public string? Deployments { get; set; } 
            public string? Plugins { get; set; } 
        }

        public class TerraformVariable
        {
            public string? Name { get; set; }
            public string? Value { get; set; }
        }

        public class TerraformVariables : List<TerraformVariable> 
        { 

        }

        /// <summary>
        /// Possible values are TRACE, DEBUG, INFO, WARN, ERROR, FATAL.
        /// </summary>
        public string? LogLevel { get; set; }

        /// <summary>
        /// Terraform working directories.
        /// </summary>
        public TerraformDirectories? Directories { get; set; }



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
