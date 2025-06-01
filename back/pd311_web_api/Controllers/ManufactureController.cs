using Microsoft.AspNetCore.Mvc;
using pd311_web_api.BLL.DTOs.Manufactures;
using pd311_web_api.BLL.Services.Manufactures;

namespace pd311_web_api.Controllers
{
    [ApiController]
    [Route("api/manufacture")]
    public class ManufactureController : BaseController
    {
        private readonly IManufactureService _manufactureService;

        public ManufactureController(IManufactureService manufactureService)
        {
            _manufactureService = manufactureService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateManufactureDto dto)
        {
            var result = await _manufactureService.CreateAsync(dto);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdateManufactureDto dto)
        {
            var result = await _manufactureService.UpdateAsync(dto);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if(!ValidateId(id, out string error))
            {
                return BadRequest(error);
            }

            var result = await _manufactureService.DeleteAsync(id);
            return result ? Ok(result) : BadRequest(result);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _manufactureService.GetAllAsync();
            return CreateActionResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok();
        }
    }
}
