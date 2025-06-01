using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pd311_web_api.BLL.DTOs.Role;
using pd311_web_api.BLL.Services.Email;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.BLL.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ILogger<RoleService> _logger;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<AppRole> roleManager, IMapper mapper, ILogger<RoleService> logger)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse> CreateAsync(CreateRoleDto dto)
        {
            var entity = _mapper.Map<AppRole>(dto);

            var result = await _roleManager.CreateAsync(entity);

            if (result.Succeeded)
            {
                return new ServiceResponse($"Роль {dto.Name} успішно створено", true);
            }

            return new ServiceResponse(result.Errors.First().Description);
        }

        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);

            if(entity == null)
            {
                return new ServiceResponse($"Роль з id {id} не знайдено");
            }

            var result = await _roleManager.DeleteAsync(entity);

            if (result.Succeeded)
            {
                return new ServiceResponse($"Роль успішно видалено", true);
            }

            return new ServiceResponse(result.Errors.First().Description);
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _roleManager.Roles.ToListAsync();

            var dtos = _mapper.Map<List<RoleDto>>(entities);

            _logger.LogInformation($"Roles received {dtos.Count}");

            return new ServiceResponse("Ролі отримано", true, dtos);
        }

        public async Task<ServiceResponse> GetByIdAsync(string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);

            if (entity == null)
                return new ServiceResponse($"Роль з id {id} не знайдено");

            var dto = _mapper.Map<RoleDto>(entity);

            return new ServiceResponse("Роль отримано", true, dto);
        }

        public async Task<ServiceResponse> UpdateAsync(RoleDto dto)
        {
            var entity = await _roleManager.FindByIdAsync(dto.Id ?? "");

            if(entity == null)
            {
                return new ServiceResponse($"Role id '{dto.Id}' not found");
            }

            var newEntity = _mapper.Map(dto, entity);

            var result = await _roleManager.UpdateAsync(newEntity);

            if(result.Succeeded)
            {
                return new ServiceResponse($"Роль {dto.Name} оновлено", true);
            }

            return new ServiceResponse(result.Errors.First().Description);
        }
    }
}
