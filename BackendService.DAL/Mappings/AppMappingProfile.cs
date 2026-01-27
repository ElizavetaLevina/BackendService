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
                .ForMember(s => s.Tags, d => d.MapFrom(c => c.Tags.Where(i => i.Deleted == false).Select(i => i.Id).ToHashSet()))
                .ForMember(s => s.Images, d => d.MapFrom(c => c.Images.Where(i => i.Deleted == false).Select(i => i.Id).ToHashSet()));

            CreateMap<PostEditDTO, PostEntity>()
                .ForMember(s => s.Tags, d => d.Ignore())
                .ForMember(s => s.Images, d => d.Ignore())
                .ForMember(s => s.DateCreate, d => d.Ignore())
                .ForMember(s => s.DateUpdate, d => d.Ignore())
                .ForMember(s => s.Deleted, d => d.Ignore());

            CreateMap<PostEntity, PostDTO>()
                .ForMember(s => s.Tags, d => d.MapFrom(c => c.Tags.Where(i => i.Deleted == false).Select(i => i.Id).ToHashSet()))
                .ForMember(s => s.Images, d => d.MapFrom(c => c.Images.Where(i => i.Deleted == false).Select(i => i.Id).ToHashSet()));

            CreateMap<TagEntity, TagEditDTO>().ReverseMap();

            CreateMap<ImageEntity, ImageViewDTO>()
                .ForMember(c => c.StringData, d => d.MapFrom(c => Convert.ToBase64String(c.Data)));
        }
    }
}
