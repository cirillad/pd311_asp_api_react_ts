using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pd311_web_api.BLL.DTOs.User;
using pd311_web_api.BLL.Services.User;

namespace pd311_web_api.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string? id, string? email, string? userName)
        {
            if(!string.IsNullOrEmpty(id))
            {
                var response = await _userService.GetByIdAsync(id);
                return CreateActionResult(response);
            }
            else if(!string.IsNullOrEmpty(email))
            {
                var response = await _userService.GetByEmailAsync(email);
                return CreateActionResult(response);
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                var response = await _userService.GetByUserNameAsync(userName);
                return CreateActionResult(response);
            }
            else
            {
                var response = await _userService.GetAllAsync();
                return CreateActionResult(response);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            var validId = ValidateId(id, out string message);
            if(!validId)
            {
                return BadRequest(message);
            }

            var response = await _userService.DeleteAsync(id ?? "");
            return CreateActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateUserDto dto)
        {
            var response = await _userService.CreateAsync(dto);
            return CreateActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdateUserDto dto)
        {
            var response = await _userService.UpdateAsync(dto);
            return CreateActionResult(response);
        }
    }
}
