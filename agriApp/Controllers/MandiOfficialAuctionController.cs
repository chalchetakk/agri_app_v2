using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using agriApp.Services.Auctions;
using agriApp.Services.Lots;
using agriApp.Dtos.Auctions;
using agriApp.Extensions; // for User.GetUserId()
using System;
using System.Threading.Tasks;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("mandiOfficialAuction")]
    public class MandiOfficialAuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;
        private readonly IArrivedLotService _arrivedLotService;

        public MandiOfficialAuctionController(
            IAuctionService auctionService,
            IArrivedLotService arrivedLotService)
        {
            _auctionService = auctionService;
            _arrivedLotService = arrivedLotService;
        }

        // ----------------------------------------------------------
        // PRE-REGISTERED LOT LIST (MANDI)
        // ----------------------------------------------------------
        [Authorize(Roles = "MANAGER,OFFICER")]
        [HttpGet("mandi/preRegisteredLots")]
        public async Task<IActionResult> GetPreRegisteredLots([FromQuery] int mandiId)
        {
            // TODO: implement PreRegisteredLotService.GetByMandi(mandiId)
            return Ok("Implement service call to fetch PreRegisteredLots for mandi");
        }

        [Authorize(Roles = "MANAGER,OFFICER")]
        [HttpGet("mandi/preRegisteredLots/{preLotId}")]
        public async Task<IActionResult> GetPreRegisteredLot(string preLotId)
        {
            return Ok("Implement service call to fetch single PreRegisteredLot detail");
        }

        // ----------------------------------------------------------
        // ARRIVED LOT LIST (MANDI)
        // ----------------------------------------------------------
        [Authorize(Roles = "MANAGER,OFFICER")]
        [HttpGet("mandi/arrivedLots")]
        public async Task<IActionResult> GetArrivedLots([FromQuery] int mandiId)
        {
            // TODO: implement ArrivedLotService.GetByMandi(mandiId)
            return Ok("Implement service call to fetch ArrivedLots for mandi");
        }

        [Authorize(Roles = "MANAGER,OFFICER")]
        [HttpGet("mandi/arrivedLots/{arrivedLotId}")]
        public async Task<IActionResult> GetArrivedLot(int arrivedLotId)
        {
            // TODO: implement ArrivedLotService.GetById(arrivedLotId)
            return Ok("Implement service call to fetch ArrivedLot detail");
        }

        // ----------------------------------------------------------
        // LIST ALL AUCTIONS FOR A MANDI
        // ----------------------------------------------------------
        [Authorize(Roles = "MANAGER,OFFICER")]
        [HttpGet("mandi/auction/all")]
        public async Task<IActionResult> GetAuctions([FromQuery] int mandiId)
        {
            var auctions = await _auctionService.GetAuctionsForMandiAsync(mandiId);
            return Ok(auctions);
        }

        // ----------------------------------------------------------
        // CREATE AUCTION (MANAGER ONLY)
        // ----------------------------------------------------------
        [Authorize(Roles = "MANAGER")]
        [HttpPost("mandi/auction/create")]
        public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionRequest dto)
        {
            dto.CreatedByOfficialId = User.GetUserId();  // auto fill manager ID

            var auction = await _auctionService.CreateAuctionAsync(dto);
            return Ok(auction);
        }

        // ----------------------------------------------------------
        // EDIT AUCTION (MANAGER ONLY)
        // ----------------------------------------------------------
        [Authorize(Roles = "MANAGER")]
        [HttpPatch("mandi/auction/{auctionId}/edit")]
        public async Task<IActionResult> EditAuction(Guid auctionId, [FromBody] EditAuctionRequest dto)
        {
            var updated = await _auctionService.EditAuctionAsync(auctionId, dto);
            return Ok(updated);
        }

        // ----------------------------------------------------------
        // LIVE AUCTION LOTS FOR AUCTION
        // ----------------------------------------------------------
        [Authorize(Roles = "MANAGER,OFFICER")]
        [HttpGet("mandi/auction/{auctionId}/liveAuctionLots")]
        public async Task<IActionResult> GetLiveLots(Guid auctionId)
        {
            var lots = await _auctionService.GetLiveLotsForAuctionAsync(auctionId);
            return Ok(lots);
        }

        [Authorize(Roles = "MANAGER,OFFICER")]
        [HttpGet("mandi/auction/liveLot/{liveAuctionLotId}")]
        public async Task<IActionResult> GetSingleLiveLot(int liveAuctionLotId)
        {
            // TODO: Create LiveAuctionLotService.GetById(liveAuctionLotId)
            return Ok("Implement LiveAuctionLotService.GetById()");
        }

        // ----------------------------------------------------------
        // UPDATE LIVE LOT STATUS (sold / unsold)
        // ----------------------------------------------------------
        [Authorize(Roles = "OFFICER")]
        [HttpPatch("mandi/auction/liveLot/{liveAuctionLotId}/status")]
        public async Task<IActionResult> UpdateLotStatus(int liveAuctionLotId, [FromBody] UpdateLiveLotStatusRequest dto)
        {
            // TODO: create LiveAuctionLotService.MarkSold/MarkUnsold
            return Ok("Implement MarkSold/MarkUnsold in LiveAuctionLotService");
        }

        // ----------------------------------------------------------
        // START AUCTION (OFFICER)
        // ----------------------------------------------------------
        [Authorize(Roles = "OFFICER")]
        [HttpPatch("mandi/auction/{auctionId}/start")]
        public async Task<IActionResult> StartAuction(Guid auctionId)
        {
            var officerId = User.GetUserId();
            var auction = await _auctionService.StartAuctionAsync(auctionId, officerId);
            return Ok(auction);
        }

        // ----------------------------------------------------------
        // END AUCTION (OFFICER)
        // ----------------------------------------------------------
        [Authorize(Roles = "OFFICER")]
        [HttpPatch("mandi/auction/{auctionId}/end")]
        public async Task<IActionResult> EndAuction(Guid auctionId)
        {
            var officerId = User.GetUserId();
            var auction = await _auctionService.EndAuctionAsync(auctionId, officerId);
            return Ok(auction);
        }
    }
}
