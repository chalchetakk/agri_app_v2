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

// ------------------------------
//        BIDS MANAGEMENT
// ------------------------------

[HttpGet("{preLotId}/bids")]
public async Task<IActionResult> GetBidsForLot(string preLotId)
{
    var result = await _lotService.GetBidsForLotAsync(
        preLotId,
        GetUserId(),
        isFarmer: false
    );

    return Ok(result);
}

[HttpPost("{preLotId}/bids/{buyerInterestLotId}/accept")]
public async Task<IActionResult> AcceptBid(string preLotId, int buyerInterestLotId)
{
    var success = await _lotService.AcceptBidAsync(
        preLotId,
        buyerInterestLotId,
        GetUserId(),
        isFarmer: false
    );

    if (!success)
        return BadRequest(new { message = "Unable to accept bid." });

    return Ok(new { message = "Bid accepted successfully." });
}

[HttpPost("{preLotId}/bids/{buyerInterestLotId}/reject")]
public async Task<IActionResult> RejectBid(string preLotId, int buyerInterestLotId)
{
    var success = await _lotService.RejectBidAsync(
        preLotId,
        buyerInterestLotId,
        GetUserId(),
        isFarmer: false
    );

    if (!success)
        return BadRequest(new { message = "Unable to reject bid." });

    return Ok(new { message = "Bid rejected successfully." });
}

[HttpGet("bids")]
public async Task<IActionResult> GetAllReceivedBids()
{
    var result = await _lotService.GetAllReceivedBidsAsync(
        GetUserId(),
        isFarmer: false
    );

    return Ok(result);
}

public class BidListItemDto
{
    public int BuyerInterestLotId { get; set; }
    public string BuyerName { get; set; } = default!;
    public string BuyerMobile { get; set; } = default!;
    public float BidAmount { get; set; }
    public string Status { get; set; } = default!; // pending/accepted/rejected
    public DateTime CreatedAt { get; set; }
}

public class ReceivedBidListItemDto
{
    public string PreLotId { get; set; } = default!;
    public int BuyerInterestLotId { get; set; }
    public float BidAmount { get; set; }
    public string Status { get; set; } = default!;
    public string BuyerName { get; set; } = default!;
    public string BuyerMobile { get; set; } = default!;
    public string CropName { get; set; } = default!;
    public string MandiName { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}

    }
}
