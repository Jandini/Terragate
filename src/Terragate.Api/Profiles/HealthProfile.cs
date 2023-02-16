using AutoMapper;

namespace Terragate.Api.Profiles
{
    public class HealthProfile : Profile
    {
        public HealthProfile() 
        {
            CreateMap<Services.HealthInfo, Models.HealthInfoDto>();
        }
    }
}
