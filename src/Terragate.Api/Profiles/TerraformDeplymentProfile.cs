using AutoMapper;

namespace Terragate.Api.Profiles
{
    public class TerraformDeplymentProfile : Profile
    {
        public TerraformDeplymentProfile()
        {
            CreateMap<Services.ITerraformDeployment, Models.DeploymentDto>();
            CreateMap<Services.ITerraformDeploymentInstance, Models.DeploymentInstanceDto>();            
        }
    }
}
