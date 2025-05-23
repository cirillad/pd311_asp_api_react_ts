using pd311_web_api.BLL.DTOs.Role;

namespace pd311_web_api.BLL.Services.Role
{
    public interface IRoleService
    {
        public Task<ServiceResponse> CreateAsync(CreateRoleDto dto);
        public Task<ServiceResponse> UpdateAsync(RoleDto dto);
        public Task<ServiceResponse> DeleteAsync(string id);
        public Task<ServiceResponse> GetByIdAsync(string id);
        public Task<ServiceResponse> GetAllAsync();
    }
}
