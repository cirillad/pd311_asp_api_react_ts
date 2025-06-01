using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using pd311_web_api.BLL.DTOs;
using pd311_web_api.DAL.Entities;
using pd311_web_api.DAL.Repositories.JwtRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.BLL.Services.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtRepository _jwtRepository;

        public JwtService(IConfiguration configuration, UserManager<AppUser> userManager, IJwtRepository jwtRepository)
        {
            _configuration = configuration;
            _userManager = userManager;
            _jwtRepository = jwtRepository;
        }

        public async Task<JwtSecurityToken?> GenerateAccessTokenAsync(AppUser user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("userId", user.Id),
                new("email", user.Email ?? ""),
                new("userName", user.UserName ?? ""),
                new("image", user.Image ?? "")
            };

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any())
            {
                var roleClaims = roles.Select(r => new Claim("role", r));
                claims.AddRange(roleClaims);
            }

            string secretKey = _configuration["JwtSettings:Key"] ?? "";
            string issuer = _configuration["JwtSettings:Issuer"] ?? "";
            string audience = _configuration["JwtSettings:Audience"] ?? "";
            int expMinutes = int.Parse(_configuration["JwtSettings:ExpTime"] ?? "");

            if (string.IsNullOrEmpty(secretKey)
                || string.IsNullOrEmpty(issuer)
                || string.IsNullOrEmpty(audience))
            {
                throw new ArgumentNullException("Jwt settings is null");
            }

            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: [.. claims],
                expires: DateTime.UtcNow.AddMinutes(expMinutes),
                signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(bytes);
            bytes = bytes.Select(b => b == 0 ? (byte)1 : b).ToArray();
            string token = Convert.ToBase64String(bytes);
            return token;
        }

        public async Task<ServiceResponse> GenerateTokensAsync(AppUser user)
        {
            var accessToken = await GenerateAccessTokenAsync(user)
                ?? throw new SecurityTokenException();
            var refreshToken = GenerateRefreshToken();

            var result = await SaveRefreshTokenAsync(user, refreshToken, accessToken.Id);

            if (!result)
            {
                throw new SecurityTokenException();
            }

            var dto = new JwtTokensDto
            {
                RefreshToken = refreshToken,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken)
            };

            return new ServiceResponse("Токени отримано", true, dto);
        }

        private async Task<bool> SaveRefreshTokenAsync(AppUser user, string refreshToken, string accessTokenId)
        {
            var entity = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                AccessId = accessTokenId,
                IsUsed = false,
                ExpiredDate = DateTime.UtcNow.AddDays(1),
                User = user,
                Token = refreshToken
            };

            return await _jwtRepository.CreateAsync(entity);
        }

        private string GetSecretKey()
        {
            return _configuration["JwtSettings:Key"]
                        ?? throw new SecurityTokenInvalidSigningKeyException("Key noy found");
        }

        private ClaimsPrincipal GetPrincipals(string accessToken, string secretKey)
        {
            var validationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidAudience = _configuration["JwtSettings:Audience"],
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"] ?? "")),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principals = tokenHandler.ValidateToken(accessToken, validationParameters, out SecurityToken securityToken);

            var jwtToken = securityToken as JwtSecurityToken;

            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                throw new SecurityTokenInvalidAlgorithmException("Invalid access token");
            }

            return principals;
        }

        public async Task<ServiceResponse> RefreshTokensAsync(JwtTokensDto dto)
        {
            var storedToken = await _jwtRepository.GetByTokenAsync(dto.RefreshToken ?? "");

            if (storedToken == null)
            {
                throw new SecurityTokenArgumentException("Refresh token not found");
            }

            if (storedToken.IsUsed)
            {
                throw new SecurityTokenArgumentException("Refresh token is used");
            }

            if (storedToken.ExpiredDate < DateTime.UtcNow)
            {
                throw new SecurityTokenExpiredException();
            }

            var principals = GetPrincipals(dto.AccessToken ?? "", GetSecretKey());

            var accessTokenId = principals.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            if (accessTokenId == null || accessTokenId != storedToken.AccessId)
            {
                throw new SecurityTokenInvalidTypeException("Token access id incorrect");
            }

            storedToken.IsUsed = true;
            await _jwtRepository.UpdateAsync(storedToken);

            var user = await _userManager.FindByIdAsync(storedToken.UserId ?? "") 
                ?? throw new ArgumentNullException("User not found");

            var tokens = await GenerateTokensAsync(user);

            return new ServiceResponse("Токени оновлено", true, tokens.Payload);
        }
    }
}
