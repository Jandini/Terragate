using AutoMapper;

namespace Terragate.Api.Profiles
{
    public class HealthProfile : Profile
    {
        public HealthProfile() 
        {
            CreateMap<Services.ElasticHealthVersion, Models.ElasticHealthVersionDto>();
            CreateMap<Services.ElasticHealthInfo, Models.ElasticHealthInfoDto>();
            CreateMap<Services.TerragateHealthInfo, Models.TerragateHealthInfoDto>();
            CreateMap<Services.HealthInfo, Models.HealthInfoDto>();
        }
    }
}
