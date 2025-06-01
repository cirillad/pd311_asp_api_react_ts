using pd311_web_api.BLL.DTOs.Car;

namespace pd311_web_api.BLL.Services.Cars
{
    public interface ICarService
    {
        Task<ServiceResponse> CreateAsync(CreateCarDto dto);
        Task<ServiceResponse> GetAllAsync(int page, int pageSize, string? manufacture);
    }
}
