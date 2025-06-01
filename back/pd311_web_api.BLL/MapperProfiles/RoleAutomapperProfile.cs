using AutoMapper;
using pd311_web_api.BLL.DTOs.Role;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.BLL.MapperProfiles
{
    public class RoleAutomapperProfile : Profile
    {
        public RoleAutomapperProfile()
        {
            // AppRole -> RoleDto
            CreateMap<AppRole, RoleDto>();

            // RoleDto -> AppRole
            CreateMap<RoleDto, AppRole>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid().ToString()));

            // AppUserRole -> RoleDto
            CreateMap<AppUserRole, RoleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ur => ur.RoleId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ur => ur.Role == null ? "no role" : ur.Role.Name));

            // CreateRoleDto -> AppRole
            CreateMap<CreateRoleDto, AppRole>();
        }
    }
}
