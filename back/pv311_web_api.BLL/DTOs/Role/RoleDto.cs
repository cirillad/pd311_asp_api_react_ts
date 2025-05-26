using FluentValidation;

namespace pv311_web_api.BLL.DTOs.Role
{
    public class RoleDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
    }
}
