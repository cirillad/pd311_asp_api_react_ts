using Microsoft.AspNetCore.Http;

namespace pd311_web_api.BLL.DTOs.Manufactures
{
    public class UpdateManufactureDto
    {
        public string Id { get; set; } = string.Empty;
        public required string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Founder { get; set; }
        public string Director { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
    }
}
