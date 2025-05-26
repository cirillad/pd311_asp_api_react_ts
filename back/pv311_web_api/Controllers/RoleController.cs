using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pv311_web_api.BLL.DTOs.Role;
using pv311_web_api.BLL.Services;
using pv311_web_api.BLL.Services.Role;

namespace pv311_web_api.Controllers
{
    [ApiController]
    [Route("api/role")]
    //[Authorize(Roles = "admin,manager", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleController : AppController
    {
        private readonly IRoleService _roleService;
        private readonly IValidator<CreateRoleDto> _createRoleValidator;
        private readonly IValidator<UpdateRoleDto> _updateRoleValidator;

        public RoleController(IRoleService roleService, IValidator<CreateRoleDto> createRoleValidator, IValidator<UpdateRoleDto> updateRoleValidator)
        {
            _roleService = roleService;
            _createRoleValidator = createRoleValidator;
            _updateRoleValidator = updateRoleValidator;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IActionResult> GetAsync(string? id)
        {
            

            var response = string.IsNullOrEmpty(id)
                ? await _roleService.GetAllAsync()
                : await _roleService.GetByIdAsync(id);

            return CreateActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateRoleDto dto)
        {
            var validResult = await _createRoleValidator.ValidateAsync(dto);

            if (!validResult.IsValid)
                return BadRequest(validResult);

            var response = await _roleService.CreateAsync(dto);
            return CreateActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateRoleDto dto)
        {
            var validResult = await _updateRoleValidator.ValidateAsync(dto);

            var isValidId = ValidateId(dto.Id, out string message);
            if (!isValidId)
                return BadRequest(message);

            if (!validResult.IsValid)
                return BadRequest(validResult);

            var response = await _roleService.UpdateAsync(dto);
            return CreateActionResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if (id == null)
            {
                return BadRequest(new ServiceResponse("Id required", false));
            }

            var isValidId = ValidateId(id, out string message);
            if (!isValidId)
                return BadRequest(message);

            var response = await _roleService.DeleteAsync(id);
            return CreateActionResult(response);
        }
    }
}
