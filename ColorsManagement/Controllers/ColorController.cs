using ColorsManagement.Dtos;
using ColorsManagement.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ColorsManagement.Controllers
{
    [Route("api/colors")]
    [ApiController]
    public class ColorController:ControllerBase
    {
        private readonly IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] OperationColorDto createColorDto)
        {
            var results = await _colorService.AddColorAsync(createColorDto);

            return StatusCode(results.StatusCode, results.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var results = await _colorService.GetAllAsync();

            return StatusCode(results.StatusCode, results.Data);
        }

        [HttpDelete("delete/{colorId}")]
        public async Task<IActionResult> Delete(Guid colorId)
        {
            var results = await _colorService.RemoveColorByIdAsync(colorId);

            return StatusCode(results.StatusCode, results.Data);
        }
        [HttpPut("update/{colorId}")]
        public async Task<IActionResult> Update(Guid colorId, [FromBody] OperationColorDto updateColorDto)
        {
            var results = await _colorService.UpdateColorAsync(colorId,updateColorDto);

            return StatusCode(results.StatusCode, results.Data);
        }

        [HttpPut("update-order")]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateDisplayOrderListDto listDto)
        {
            var results = await _colorService.UpdateColorOrderAsync(listDto);

            return StatusCode(results.StatusCode, results.Data);
        }

    }
}
