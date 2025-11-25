using agriApp.Services.Crops;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("crops")]
    [Authorize]
    public class CropController : ControllerBase
    {
        private readonly ICropService _cropService;

        public CropController(ICropService cropService)
        {
            _cropService = cropService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCrops()
        {
            var result = await _cropService.GetAllCropsAsync();
            return Ok(result);
        }
    }

    public class CropDto
{
    public int CropId { get; set; }
    public string CropName { get; set; } = default!;
    public string? Grade { get; set; }
}

}
