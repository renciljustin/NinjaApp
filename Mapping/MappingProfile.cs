using AutoMapper;
using NinjaApp.Core.Models;
using NinjaApp.Persistence.Dtos;

namespace NinjaApp.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDetailDto>();
            CreateMap<User, UserListDto>();

            CreateMap<UserRegisterDto, User>();
            CreateMap<UserDetailDto, User>();
        }
    }
}