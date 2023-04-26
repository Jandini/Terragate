
namespace Terragate.Api.Services
{
    internal class TerraformInfrastructureNotFoundException : Exception
    {

        public TerraformInfrastructureNotFoundException(string? message) : base(message)
        {
        }
    }
}