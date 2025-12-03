using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using agriApp.Services.Lots;
using System;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("mandi-official/auction")]
    [Authorize(Roles = "OFFICER")]
    public class MandiAuctionController : ControllerBase
    {
        private readonly ILiveAuctionLotService _auctionService;

        public MandiAuctionController(ILiveAuctionLotService auctionService)
        {
            _auctionService = auctionService;
        }

        public class MarkSoldRequest
        {
            public float FinalPrice { get; set; }
            public Guid? BuyerId { get; set; }           // app buyer (Guid)
            public string? BuyerName { get; set; }       // offline buyer
            public string? BuyerMobile { get; set; }     // offline buyer
        }

        [HttpPost("{liveAuctionLotId}/sold")]
        public async Task<IActionResult> MarkSold(int liveAuctionLotId, [FromBody] MarkSoldRequest req)
        {
            if (req.FinalPrice <= 0)
                return BadRequest("Final price must be greater than zero.");

            var result = await _auctionService.MarkSoldAsync(
                liveAuctionLotId,
                req.FinalPrice,
                req.BuyerId,
                req.BuyerName,
                req.BuyerMobile
            );

            return result == null ? NotFound("Auction lot not found.") : Ok(result);
        }

        [HttpPost("{liveAuctionLotId}/unsold")]
        public async Task<IActionResult> MarkUnsold(int liveAuctionLotId)
        {
            var result = await _auctionService.MarkUnsoldAsync(liveAuctionLotId);
            return result == null ? NotFound("Auction lot not found.") : Ok(result);
        }
    }
}
