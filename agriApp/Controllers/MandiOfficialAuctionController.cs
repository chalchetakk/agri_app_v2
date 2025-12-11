using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using agriApp.Services.Auctions;
using agriApp.Services.Lots;
using agriApp.Services.MandiOfficials;
using agriApp.Dtos.Auctions;
using agriApp.DTOs.MandiOfficials;
using agriApp.Extensions; // for User.GetUserId()
using System;
using System.Threading.Tasks;
using agriApp.Dtos.Lots;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("mandiOfficialAuction")]
    public class MandiOfficialAuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;
        private readonly IArrivedLotService _arrivedLotService;
private readonly IPreRegisteredLotQueryService _preLotQuery;  

private readonly IArrivedLotQueryService _arrivedLotQuery;
private readonly ILiveAuctionLotService _liveAuctionLotService;

private readonly IMandiOfficialService _mandiOfficialService;
public MandiOfficialAuctionController(
            IAuctionService auctionService, 
            IArrivedLotService arrivedLotService,
            IPreRegisteredLotQueryService preLotQuery,
            IArrivedLotQueryService arrivedLotQuery,
            ILiveAuctionLotService liveAuctionLotService,
            IMandiOfficialService mandiOfficialService)
        {
            _auctionService = auctionService;
            _arrivedLotService = arrivedLotService;
            _preLotQuery = preLotQuery;
            _arrivedLotQuery = arrivedLotQuery;
            _liveAuctionLotService = liveAuctionLotService;
            _mandiOfficialService = mandiOfficialService;
        }

        // ----------------------------------------------------------
        // PRE-REGISTERED LOT LIST (MANDI)
        // ----------------------------------------------------------
        [Authorize(Roles = "MANAGER,OFFICER")]
        [HttpGet("mandi/preRegisteredLots")]
        public async Task<IActionResult> GetPreRegisteredLots([FromQuery] int mandiId)
        {
            // TODO: implement PreRegisteredLotService.GetByMandi(mandiId)
            // return Ok("Implement service call to fetch PreRegisteredLots for mandi");
            var lots = await _preLotQuery.GetLotsForMandiAsync(mandiId);
    return Ok(lots);
        }

        [Authorize(Roles = "MANAGER,OFFICER")]
        [HttpGet("mandi/preRegisteredLots/{preLotId}")]
        public async Task<IActionResult> GetPreRegisteredLot(string preLotId)
        {
            // return Ok("Implement service call to fetch single PreRegisteredLot detail");
            var lot = await _preLotQuery.GetLotByIdAsync(preLotId);
    if (lot == null) return NotFound();
    return Ok(lot);
        }

        // ----------------------------------------------------------
        // ARRIVED LOT LIST (MANDI)
        // ----------------------------------------------------------
        [Authorize(Roles = "MANAGER,APPROVER,OFFICER")]
        [HttpGet("mandi/arrivedLots")]
        public async Task<IActionResult> GetArrivedLots([FromQuery] int mandiId)
        {
            // TODO: implement ArrivedLotService.GetByMandi(mandiId)
            // return Ok("Implement service call to fetch ArrivedLots for mandi");
        
            var list = await _arrivedLotQuery.GetArrivedLotsForMandiAsync(mandiId);
    return Ok(list);
        }

        [Authorize(Roles = "MANAGER,APPROVER,OFFICER")]
        [HttpGet("mandi/arrivedLots/{arrivedLotId}")]
        public async Task<IActionResult> GetArrivedLot(int arrivedLotId)
        {
            // TODO: implement ArrivedLotService.GetById(arrivedLotId)
            // return Ok("Implement service call to fetch ArrivedLot detail");
            var lot = await _arrivedLotQuery.GetArrivedLotByIdAsync(arrivedLotId);
    if (lot == null) return NotFound();

    return Ok(lot);
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
            dto.CreatedByOfficialId = User.GetOfficialId();  // auto fill manager ID

            var auction = await _auctionService.CreateAuctionAsync(dto);
            
            // FIX: convert entity → DTO
    var response = AuctionDtoMapper.ToDetailDto(auction);
            return Ok(response);
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
            // return Ok("Implement LiveAuctionLotService.GetById()");
             var result = await _liveAuctionLotService.GetByIdAsync(liveAuctionLotId);
    if (result == null)
        return NotFound("Lot not found");

    return Ok(result);
        }

        // ----------------------------------------------------------
        // UPDATE LIVE LOT STATUS (sold / unsold)
        // ----------------------------------------------------------
        [Authorize(Roles = "OFFICER")]
        [HttpPatch("mandi/auction/liveLot/{liveAuctionLotId}/status")]
        public async Task<IActionResult> UpdateLiveLotStatus(int liveAuctionLotId, [FromBody] UpdateLiveLotStatusRequest dto)
{
    LiveAuctionLotDto result;

    if (dto.Status.ToLower() == "sold")
    {
        result = await _liveAuctionLotService.MarkSoldAsync(
            liveAuctionLotId,
            dto.FinalPrice ?? 0,
            dto.BuyerId,
            dto.BuyerName,
            dto.BuyerMobile
        );
    }
    else if (dto.Status.ToLower() == "unsold")
    {
        result = await _liveAuctionLotService.MarkUnsoldAsync(liveAuctionLotId);
    }
    else
    {
        return BadRequest("Invalid status. Allowed: sold, unsold");
    }

    return Ok(result);
}

        // ----------------------------------------------------------
        // START AUCTION (OFFICER)
        // ----------------------------------------------------------
        [Authorize(Roles = "OFFICER")]
        [HttpPatch("mandi/auction/{auctionId}/start")]
        public async Task<IActionResult> StartAuction(Guid auctionId)
        {
            var officerId = User.GetOfficialId();
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
            // dto.CreatedByOfficialId = User.GetUserId();
// Console.WriteLine("JWT OfficialId = " + User.GetOfficialId());

            var officerId = User.GetOfficialId();
            var auction = await _auctionService.EndAuctionAsync(auctionId, officerId);
            return Ok(auction);
        }

        [Authorize(Roles = "MANAGER")]
[HttpGet("mandi/mandiOfficersList")]
public async Task<IActionResult> GetMandiOfficers([FromQuery] int mandiId)
{
    var officers = await _mandiOfficialService.GetOfficersByMandiIdAsync(mandiId);
    return Ok(officers);
}

    }
}
