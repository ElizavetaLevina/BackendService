using AutoMapper;
using BackendService.DAL.Models;
using BackendService.Common.DTO;

namespace BackendService.DAL.Mappings
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile() 
        {
            CreateMap<PostEntity, PostEditDTO>()
                .ForMember(s => s.Tags, d => d.MapFrom(c => c.Tags.Select(i => i.Id).ToHashSet()));

            CreateMap<PostEditDTO, PostEntity>()
                .ForMember(s => s.Tags, d => d.Ignore())
                .ForMember(s => s.DateCreate, d => d.Ignore())
                .ForMember(s => s.DateUpdate, d => d.Ignore())
                .ForMember(s => s.Deleted, d => d.Ignore());

            CreateMap<PostEntity, PostDTO>()
                .ForMember(s => s.Tags, d => d.MapFrom(c => c.Tags.Select(i => i.Id).ToHashSet()));

            CreateMap<TagEntity, TagEditDTO>().ReverseMap();
        }
    }
}
