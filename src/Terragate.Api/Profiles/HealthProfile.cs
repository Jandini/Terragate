using AutoMapper;

namespace Terragate.Api.Profiles
{
    public class HealthProfile : Profile
    {
        public HealthProfile() 
        {
            CreateMap<Services.ElasticInfo, Models.ElasticInfoDto>()
                .ForMember(d => d.Version, a => a.MapFrom(s => s.Version!.Number))
                .ForMember(d => d.Type, a => a.MapFrom(s => s.Version!.BuildType))
                .ForMember(d => d.Cluster, a => a.MapFrom(s => s.ClusterName));
            CreateMap<Services.TerragateInfo, Models.TerragateInfoDto>();
            CreateMap<Services.HealthInfo, Models.HealthInfoDto>();
        }
    }
}
