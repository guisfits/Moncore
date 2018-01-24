using AutoMapper;
using Moncore.Api.Models;
using Moncore.Domain.Entities;

namespace Moncore.Api.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserForCreatedDto, User>();
        }
    }
}
