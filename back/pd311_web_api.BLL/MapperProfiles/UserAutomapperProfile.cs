using AutoMapper;
using pd311_web_api.BLL.DTOs.Account;
using pd311_web_api.BLL.DTOs.Role;
using pd311_web_api.BLL.DTOs.User;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.BLL.MapperProfiles
{
    public class UserAutomapperProfile : Profile
    {
        public UserAutomapperProfile()
        {
            // RegisterDto -> AppUser
            CreateMap<RegisterDto, AppUser>();

            // AppUser -> UserDto
            CreateMap<AppUser, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles));

            // CreateUserDto -> AppUser
            CreateMap<CreateUserDto, AppUser>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            // UpdateUserDto -> AppUser
            CreateMap<UpdateUserDto, AppUser>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}
