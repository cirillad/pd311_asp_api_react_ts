using pd311_web_api.BLL.DTOs.Manufactures;

namespace pd311_web_api.BLL.Services.Manufactures
{
    public interface IManufactureService
    {
        Task<bool> CreateAsync(CreateManufactureDto dto);
        Task<bool> UpdateAsync(UpdateManufactureDto dto);
        Task<bool> DeleteAsync(string id);
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> GetByIdAsync(string id);
    }
}
