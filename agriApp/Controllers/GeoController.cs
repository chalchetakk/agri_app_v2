using agriApp.Services.Geography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("geo")]
    [Authorize] 
    public class GeoController : ControllerBase
    {
        private readonly IGeoService _geoService;

        public GeoController(IGeoService geoService)
        {
            _geoService = geoService;
        }

        // GET /geo/states
        [HttpGet("states")]
        public async Task<IActionResult> GetStates()
        {
            var states = await _geoService.GetStatesAsync();
            return Ok(states);
        }

        // GET /geo/districts?state=Maharashtra
        [HttpGet("districts")]
        public async Task<IActionResult> GetDistricts([FromQuery] string state)
        {
            if (string.IsNullOrWhiteSpace(state))
                return BadRequest("State is required");

            var districts = await _geoService.GetDistrictsByStateAsync(state);
            return Ok(districts);
        }
    }
}
