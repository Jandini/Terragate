namespace Terragate.Api.Services
{
    public class TerraformDeploymentInstance : ITerraformDeploymentInstance
    {
        public string HostName { get; set;}

        public string IpAddress { get; set;}

        public TerraformDeploymentInstance(string hostName, string ipAddress)
        {
            HostName = hostName;    
            IpAddress = ipAddress;
        }
    }
}
