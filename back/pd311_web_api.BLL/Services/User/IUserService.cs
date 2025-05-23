using pd311_web_api.BLL.DTOs.User;

namespace pd311_web_api.BLL.Services.User
{
    public interface IUserService
    {
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> GetByIdAsync(string id);
        Task<ServiceResponse> GetByUserNameAsync(string userName);
        Task<ServiceResponse> GetByEmailAsync(string email);
        Task<ServiceResponse> CreateAsync(CreateUserDto dto);
        Task<ServiceResponse> UpdateAsync(UpdateUserDto dto);
        Task<ServiceResponse> DeleteAsync(string id);
    }
}
