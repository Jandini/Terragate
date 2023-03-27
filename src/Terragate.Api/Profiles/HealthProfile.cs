using AutoMapper;

namespace Terragate.Api.Profiles
{
    public class HealthProfile : Profile
    {
        public HealthProfile() 
        {
            CreateMap<Services.ElasticVersion, Models.ElasticVersionDto>();
            CreateMap<Services.ElasticInfo, Models.ElasticInfoDto>();
            CreateMap<Services.TerragateInfo, Models.TerragateInfoDto>();
            CreateMap<Services.HealthInfo, Models.HealthInfoDto>();
        }
    }
}
