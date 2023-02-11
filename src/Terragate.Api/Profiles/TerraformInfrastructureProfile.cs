using AutoMapper;

namespace Terragate.Api.Profiles
{
    public class TerraformInfrastructureProfile : Profile
    {
        public TerraformInfrastructureProfile()
        {
            CreateMap<Services.ITerraformInfrastructure, Models.InfrastructureDto>();
            CreateMap<Services.ITerraformInfrastructureResource, Models.InfrastructureResrouceDto>();
            CreateMap<Services.ITerraformInfrastructureResourceInstance, Models.InfrastructureResourceInstanceDto>();
        }
    }
}
