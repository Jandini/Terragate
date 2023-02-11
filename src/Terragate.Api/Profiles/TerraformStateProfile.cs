using AutoMapper;

namespace Terragate.Api.Profiles
{
    public class TerraformStateProfile : Profile
    {
        public TerraformStateProfile()
        {
            CreateMap<Services.TerraformState.Instance, Services.TerraformInfrastructureResourceInstance>()
                .ForMember(dest => dest.HostName, opt => opt.MapFrom(src => src.Name))
                .ValidateMemberList(MemberList.None);

            CreateMap<Services.TerraformState.Attributes, Services.TerraformInfrastructureResourceInstance>()
                .ValidateMemberList(MemberList.None);

            CreateMap<Services.TerraformState.Properties, Services.TerraformInfrastructureResourceInstance>()
                .ValidateMemberList(MemberList.None);

        }
    }
}
