using AutoMapper;
using AutoMapper.Execution;
using BackendService.Common.DTO;
using BackendService.DAL.Models;
using Shared.Contracts.DTO;
using System.Collections.Generic;

namespace BackendService.DAL.Mappings
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile() 
        {
            CreateMap<PostEntity, PostEditDTO>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(c => c.Tags.Where(i => i.Deleted == false).Select(i => i.Id).ToHashSet()))
                .ForMember(d => d.Images, opt => opt.MapFrom(c => c.Images.Where(i => i.Deleted == false).Select(i => i.Id).ToHashSet()));

            CreateMap<PostEditDTO, PostEntity>()
                .ForMember(d => d.UserId, opt => opt.Ignore())
                .ForMember(d => d.Tags, opt => opt.Ignore())
                .ForMember(d => d.Images, opt => opt.Ignore())
                .ForMember(d => d.DateCreate, opt => opt.Ignore())
                .ForMember(d => d.DateUpdate, opt => opt.Ignore())
                .ForMember(d => d.Deleted, opt => opt.Ignore())
                .ForMember(d => d.PendingVersion, opt => opt.Ignore())
                .ForMember(d => d.PostPendingId, opt => opt.Ignore());

            CreateMap<PostEntity, PostDTO>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(c => c.Tags.Where(i => i.Deleted == false).Select(i => i.Id).ToHashSet()))
                .ForMember(d => d.Images, opt => opt.MapFrom(c => c.Images.Where(i => i.Deleted == false).Select(i => i.Id).ToHashSet()));

            CreateMap<TagEntity, TagEditDTO>().ReverseMap();

            CreateMap<ImageEntity, ImageViewDTO>()
                .ForMember(c => c.StringData, opt => opt.MapFrom(c => Convert.ToBase64String(c.Data)));

            CreateMap<PostPendingEditDTO, PostPendingEntity>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.DateCreate, opt => opt.Ignore())
                .ForMember(d => d.DateModerate, opt => opt.Ignore())
                .ForMember(d => d.Status, opt => opt.Ignore())
                .ForMember(d => d.RejectionReason, opt => opt.Ignore())
                .ForMember(d => d.UserId, opt => opt.Ignore());

            CreateMap<PostPendingEntity, PostPendingEditDTO>();

            CreateMap<PostPendingEditDTO, PostSubmittedForModeration>()
                .ForMember(d => d.DateCreate, opt => opt.Ignore())
                .ForMember(d => d.UserId, opt => opt.Ignore());

            CreateMap<PostPendingEntity, PostPendingViewDTO>();

            CreateMap<PostPendingViewDTO, PostEditDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(c => c.PostId))
                .ForMember(d => d.Tags, opt => opt.MapFrom(c => c.TagIds))
                .ForMember(d => d.Images, opt => opt.MapFrom(c => c.ImageIds));
        }
    }
}
