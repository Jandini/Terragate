namespace Terragate.Api.Services
{
    public class TerraformProcessOptions
    {
        public TerraformProcessOptions()
        {            

        }

        public TerraformProcessOptions(Dictionary<string, object> variables)
        {
            Variables = variables;
        }

        public TerraformProcessOptions(string arguments, Dictionary<string, object> variables)
        {
            Arguments = arguments;
            Variables = variables;
        }

        public string? Arguments { get; set; } 
        public Dictionary<string, object>? Variables { get; set; } 
        public DirectoryInfo? WorkingDirectory { get; set; }
    }
}
