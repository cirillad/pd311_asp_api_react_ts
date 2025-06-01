using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using pd311_web_api.BLL.DTOs;
using pd311_web_api.BLL.DTOs.Account;
using pd311_web_api.BLL.Services.Email;
using pd311_web_api.BLL.Services.JwtService;
using StackExchange.Redis;
using System.Text;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.BLL.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        //private readonly IDatabase _redisDb;

        private readonly IMapper _mapper;

        public AccountService(UserManager<AppUser> userManager, IEmailService emailService, IMapper mapper, RoleManager<AppRole> roleManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _mapper = mapper;
            _roleManager = roleManager;
            _jwtService = jwtService;
            //_redisDb = redisDb;
        }

        public async Task<bool> ConfirmEmailAsync(string id, string base64)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var bytes = Convert.FromBase64String(base64);
                var token = Encoding.UTF8.GetString(bytes);
                var result = await _userManager.ConfirmEmailAsync(user, token);
                return result.Succeeded;
            }

            return false;
        }

        public async Task<ServiceResponse> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName ?? "");

            if (user == null)
                return new ServiceResponse($"Користувача з іменем '{dto.UserName}' не знайдено");

            var result = await _userManager.CheckPasswordAsync(user, dto.Password ?? "");

            if (!result)
                return new ServiceResponse($"Пароль вказано невірно");

            // Generate jwt token
            var response = await _jwtService.GenerateTokensAsync(user);

            var tokens = (JwtTokensDto?)response.Payload;

            //_redisDb.StringSet($"atoken:{user.Id}", tokens.AccessToken, TimeSpan.FromMinutes(1));
            //_redisDb.StringSet($"rtoken:{user.Id}", tokens.RefreshToken, TimeSpan.FromMinutes(60));

            return new ServiceResponse("Успішний вхід", true, tokens);
        }

        public async Task<ServiceResponse> RegisterAsync(RegisterDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return new ServiceResponse($"Email '{dto.Email}' зайнятий");

            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                return new ServiceResponse($"Ім'я '{dto.UserName}' вже використовується");

            var user = _mapper.Map<AppUser>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return new ServiceResponse(result.Errors.First().Description);
            }

            if (result.Succeeded && await _roleManager.RoleExistsAsync("user"))
            {
                result = await _userManager.AddToRoleAsync(user, "user");
            }

            if (!result.Succeeded)
                return new ServiceResponse(result.Errors.First().Description);

            await SendConfirmEmailTokenAsync(user.Id);

            // Generate jwt token
            var tokens = await _jwtService.GenerateTokensAsync(user);

            return new ServiceResponse("Успішна реєстрація", true, tokens);
        }

        public async Task<bool> SendConfirmEmailTokenAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return false;
                }

                // Sent mail
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var bytes = Encoding.UTF8.GetBytes(token);
                var base64 = Convert.ToBase64String(bytes);

                var body = $"<a href='https://localhost:7223/api/account/confirmEmail?id={user.Id}&t={base64}'>Підтвердити пошту</a>";

                await _emailService.SendMailAsync(user.Email!, "Email confirm", body, true);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
