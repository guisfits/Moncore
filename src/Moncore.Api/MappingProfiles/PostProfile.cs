using AutoMapper;
using Moncore.Api.Models;
using Moncore.Domain.Entities;

namespace Moncore.Api.MappingProfiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostForCreatedDto, Post>();
            CreateMap<Post, PostDto>();
            CreateMap<Post, PostsByUserDto>();
        }
    }
}
