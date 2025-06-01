using pd311_web_api.BLL.DTOs;
using System.IdentityModel.Tokens.Jwt;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.BLL.Services.JwtService
{
    public interface IJwtService
    {
        string GenerateRefreshToken();
        Task<JwtSecurityToken?> GenerateAccessTokenAsync(AppUser user);
        Task<ServiceResponse> GenerateTokensAsync(AppUser user);
        Task<ServiceResponse> RefreshTokensAsync(JwtTokensDto dto);
    }
}
