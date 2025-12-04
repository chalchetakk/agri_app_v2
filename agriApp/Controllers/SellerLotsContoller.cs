using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using agriApp.Services.Lots;
using agriApp.Dtos.Lots;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("seller/lots")]
    [Authorize]
    public class SellerLotsController : ControllerBase
    {
        private readonly ILotService _lotService;

        public SellerLotsController(ILotService lotService)
        {
            _lotService = lotService;
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterLot([FromForm] LotRegisterRequest request)
        {
            var result = await _lotService.RegisterLotAsync(GetUserId(), request, isFarmer: false);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetMyLots()
        {
            var result = await _lotService.GetMyLotsAsync(GetUserId(), isFarmer: false);
            return Ok(result);
        }

        [HttpGet("{preLotId}")]
        public async Task<IActionResult> GetLotDetails(string preLotId)
        {
            var result = await _lotService.GetLotByIdAsync(preLotId, GetUserId(), isFarmer: false);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPut("{preLotId}")]
public async Task<IActionResult> EditLot(string preLotId, [FromForm] LotEditRequest request)
{
    var userId = GetUserId();
    var result = await _lotService.EditLotAsync(preLotId, userId, isFarmer: false, request);

    if (result == null)
        return NotFound(new { message = "Lot not found." });

    return Ok(result);
}


[HttpDelete("{preLotId}")]
public async Task<IActionResult> DeleteLot(string preLotId)
{
    var userId = GetUserId();

    var result = await _lotService.DeleteLotAsync(preLotId, userId, isFarmer: false);
    if (!result)
        return NotFound(new { message = "Lot not found or not owned by you." });

    return Ok(new { message = "Lot deleted successfully." });
}

// SellerLotsController.cs

[HttpGet("auction/lots")]
public async Task<IActionResult> GetMyAuctionLots()
{
    var result = await _lotService.GetMyAuctionLotsAsync(GetUserId(), isFarmer: false);
    return Ok(result);
}

[HttpGet("auction/lots/{arrivedLotId}")]
public async Task<IActionResult> GetMyAuctionLot(int arrivedLotId)
{
    var result = await _lotService.GetMyAuctionLotAsync(arrivedLotId, GetUserId(), isFarmer: false);
    return result == null ? NotFound() : Ok(result);
}


    }
}
