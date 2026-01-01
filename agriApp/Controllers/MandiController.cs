using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using agriApp.Services.Mandis;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("mandis")]
    [Authorize]
    public class MandiController : ControllerBase
    {
        private readonly IMandiService _mandiService;

        public MandiController(IMandiService mandiService)
        {
            _mandiService = mandiService;
        }

        // [HttpGet]
        // public async Task<IActionResult> GetMandis()
        // {
        //     var list = await _mandiService.GetMandisAsync();
        //     return Ok(list);
        // }
         // ✅ GET /mandis
        // ✅ GET /mandis?district=Pune
        [HttpGet]
        public async Task<IActionResult> GetMandis([FromQuery] string? district)
        {
            var list = await _mandiService.GetMandisAsync(district);
            return Ok(list);
        }
    }
    public class MandiDto
{
    public int MandiId { get; set; }
    public string MandiName { get; set; } = default!;
    public string Location { get; set; } = default!;

    public string District { get; set; } = default!;
}

}
