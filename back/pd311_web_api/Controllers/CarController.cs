using Microsoft.AspNetCore.Mvc;
using pd311_web_api.BLL.DTOs.Car;
using pd311_web_api.BLL.Services.Cars;

namespace pd311_web_api.Controllers
{
    [ApiController]
    [Route("api/car")]
    public class CarController : BaseController
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCarDto dto)
        {
            var response = await _carService.CreateAsync(dto);
            return CreateActionResult(response);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync(int page = 1, int pageSize = 3, string? manufacture = null)
        {
            var response = await _carService.GetAllAsync(page, pageSize, manufacture);
            return CreateActionResult(response);
        }

        [HttpPut]
        public IActionResult UpdateAsync()
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteAsync()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAsync()
        {
            return Ok();
        }
    }
}
