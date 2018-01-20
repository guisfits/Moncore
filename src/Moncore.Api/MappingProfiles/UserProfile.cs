using AutoMapper;
using Moncore.Api.Models;
using Moncore.Domain.Entities.UserAggregate;

namespace Moncore.Api.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
