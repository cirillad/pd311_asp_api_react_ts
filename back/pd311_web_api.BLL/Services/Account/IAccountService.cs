using pd311_web_api.BLL.DTOs.Account;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.BLL.Services.Account
{
    public interface IAccountService
    {
        public Task<ServiceResponse> LoginAsync(LoginDto dto);
        public Task<ServiceResponse> RegisterAsync(RegisterDto dto);
        public Task<bool> ConfirmEmailAsync(string id, string token);
        public Task<bool> SendConfirmEmailTokenAsync(string userId);
    }
}
