using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using agriApp.Services.Lots;
using agriApp.DTOs.Lots;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("buyer/lots")]
    [Authorize]
    public class BuyerLotsController : ControllerBase
    {
        private readonly IBuyerInterestLotService _buyerService;
        private readonly IBuyerLotRecommendationService _recommendationService;

        public BuyerLotsController(
            IBuyerInterestLotService buyerService,
            IBuyerLotRecommendationService recommendationService)
        {
            _buyerService = buyerService;
            _recommendationService = recommendationService;
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
        }

        // 1️⃣ Get lots available for buyer (Recommendation service)
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableLots()
        {
            var result = await _recommendationService.GetRecommendedLotsForBuyerAsync(GetUserId());
            return Ok(result); 
        }

        // 2️⃣ Get lot details for buyer
        [HttpGet("{preLotId}")]
        public async Task<IActionResult> GetLotDetails(string preLotId)
        {
            var result = await _buyerService.GetLotDetailForBuyerAsync(preLotId, GetUserId());
            return result == null ? NotFound() : Ok(result);
        }

        // 3️⃣ Buyer places a bid
        [HttpPost("{preLotId}/bid")]
        public async Task<IActionResult> PlaceBid(string preLotId, [FromBody] PlaceBidDto request)
        {
            var result = await _buyerService.PlaceBidAsync(GetUserId(), preLotId, request.BidAmount);
            return Ok(result);
        }

        // 4️⃣ Buyer sees list of bids he placed
        [HttpGet("bids")]
        public async Task<IActionResult> GetMyBids()
        {
            var result = await _buyerService.GetMyPlacedBidsAsync(GetUserId());
            return Ok(result);
        }

// 5️⃣ Buyer sees details of a specific bid
[HttpGet("bids/{buyerInterestLotId}")]
public async Task<IActionResult> GetMyBidDetail(int buyerInterestLotId)
{
    var result = await _buyerService.GetMyBidDetailAsync(
        buyerInterestLotId,
        GetUserId());

    return result == null ? NotFound() : Ok(result);
}


        // DTOs inside controller
        public class PlaceBidDto
        {
            public decimal BidAmount { get; set; }
        }

        public class BuyerLotListItemDto
        {
            public string PreLotId { get; set; } = default!;
            public string CropName { get; set; } = default!;
            public float Quantity { get; set; }
            public string? Grade { get; set; }
            public string? LotImageUrl { get; set; }
            public string? Status { get; set; }
            public int MandiId { get; set; }
            public string MandiName { get; set; } = default!;
        }

        public class BuyerLotDetailDto
        {
            public string PreLotId { get; set; } = default!;
            public string CropName { get; set; } = default!;
            public float Quantity { get; set; }
            public string? Grade { get; set; }
            public string? LotImageUrl { get; set; }
            public string? QrCodeUrl { get; set; }
            public string Status { get; set; } = default!;
            public int MandiId { get; set; }
            public string MandiName { get; set; } = default!;
            public decimal? MyBidAmount { get; set; }
            public string? MyBidStatus { get; set; }
        }

        public class BuyerPlacedBidDto
        {
            public int BuyerInterestLotId { get; set; }
            public string PreLotId { get; set; } = default!;
            public decimal BidAmount { get; set; }
            public string Status { get; set; } = default!;
            public DateTime CreatedAt { get; set; }
        }
    }
}
