using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using agriApp.Services.Lots;
using agriApp.Data;
using agriApp.Entities.Lots;
using Microsoft.EntityFrameworkCore;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("mandi-official/lots")]
    [Authorize(Roles = "OFFICER,APPROVER")]
    public class MandiOfficialLotsController : ControllerBase
    {
        private readonly AgriDbContext _db;
        private readonly IArrivedLotService _arrivedLotService;
        private readonly ILiveAuctionLotService _auctionService;

        public MandiOfficialLotsController(
            AgriDbContext db,
            IArrivedLotService arrivedLotService,
            ILiveAuctionLotService auctionService)
        {
            _db = db;
            _arrivedLotService = arrivedLotService;
            _auctionService = auctionService;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirst("sub")!.Value);

        // ----------------------------------------------------
        // 1. Search owner
        // ----------------------------------------------------
        [HttpGet("search-owner")]
        public async Task<IActionResult> SearchOwner([FromQuery] string keyword)
        {
            keyword = keyword?.Trim() ?? "";

            var farmers = await _db.Farmers
                .Include(f => f.User)
                .Where(x =>
                    (x.FarmerName != null && EF.Functions.Like(x.FarmerName, $"%{keyword}%")) ||
                    (x.User != null && EF.Functions.Like(x.User.MobileNumber, $"%{keyword}%"))
                )
                .Select(x => new {
                    role = "farmer",
                    id = x.FarmerId,
                    name = x.FarmerName,
                    mobile = x.User != null ? x.User.MobileNumber : ""
                })
                .ToListAsync();

            var sellers = await _db.Sellers
                .Include(s => s.User)
                .Where(x =>
                    (x.SellerName != null && EF.Functions.Like(x.SellerName, $"%{keyword}%")) ||
                    (x.User != null && EF.Functions.Like(x.User.MobileNumber, $"%{keyword}%"))
                )
                .Select(x => new {
                    role = "seller",
                    id = x.SellerId,
                    name = x.SellerName,
                    mobile = x.User != null ? x.User.MobileNumber : ""
                })
                .ToListAsync();

            return Ok(farmers.Concat<object>(sellers));
        }


        // ----------------------------------------------------
// 2. Create ArrivedLot (manual entry)
// ----------------------------------------------------
[HttpPost("arrived/create")]
public async Task<IActionResult> CreateArrivedLot([FromBody] ArrivedLotCreateDto dto)
{
    // ✔ Fetch mandi official entry
    var userId = GetUserId();
    var official = await _db.MandiOfficials
        .FirstOrDefaultAsync(x => x.UserId == userId);

    if (official == null)
        return Unauthorized("You are not registered as a Mandi Official.");

    var lot = new ArrivedLot
    {
        MandiId = dto.MandiId,
        LotOwnerRole = dto.LotOwnerRole,
        LotOwnerName = dto.LotOwnerName,
        MobileNum = dto.MobileNum,
        FarmerId = dto.FarmerId,
        SellerId = dto.SellerId,
        PreLotId = dto.PreLotId,
        CropId = dto.CropId,
        Quantity = dto.Quantity,
        Grade = dto.Grade,
        LotImageUrl = dto.LotImageUrl,
        Status = "arrived",

        // ❌ OLD: MandiOfficerId = GetUserId(),
        // ✔ NEW:
        MandiOfficerId = official.OfficialId
    };

    var result = await _arrivedLotService.CreateArrivedLotAsync(lot);

    return Ok(new ArrivedLotResponseDto(result));
}



// ----------------------------------------------------
// 3. Create ArrivedLot from preLotId
// ----------------------------------------------------
[HttpGet("arrived/from-prelot")]
public async Task<IActionResult> CreateFromPreLot([FromQuery] string preLotId)
{
    // ✔ Fetch mandi official entry
    var userId = GetUserId();
    var official = await _db.MandiOfficials
        .FirstOrDefaultAsync(x => x.UserId == userId);

    if (official == null)
        return Unauthorized("You are not registered as a Mandi Official.");

    var preLot = await _db.PreRegisteredLots
        .Include(x => x.Farmer).ThenInclude(f => f.User)
        .Include(x => x.Seller).ThenInclude(s => s.User)
        .FirstOrDefaultAsync(x => x.PreLotId == preLotId);

    if (preLot == null)
        return NotFound("Invalid PreLotId.");

    var lot = new ArrivedLot
    {
        MandiId = preLot.MandiId,
        CropId = preLot.CropId,
        Quantity = preLot.Quantity,
        Grade = preLot.Grade,
        LotImageUrl = preLot.LotImageUrl,
        PreLotId = preLot.PreLotId,
        FarmerId = preLot.FarmerId,
        SellerId = preLot.SellerId,

        LotOwnerRole = preLot.FarmerId != null ? "farmer" : "seller",
        LotOwnerName = preLot.Farmer?.FarmerName ?? preLot.Seller?.SellerName ?? "",
        MobileNum = preLot.Farmer?.User?.MobileNumber ?? preLot.Seller?.User?.MobileNumber ?? "",

        Status = "arrived",

        // ❌ OLD: MandiOfficerId = GetUserId()
        // ✔ NEW:
        MandiOfficerId = official.OfficialId
    };

    var result = await _arrivedLotService.CreateArrivedLotAsync(lot);
    return Ok(new ArrivedLotResponseDto(result));
}


        // ----------------------------------------------------
        // 4. Edit ArrivedLot
        // ----------------------------------------------------
        [HttpPut("arrived/{arrivedLotId}/edit")]
        public async Task<IActionResult> EditArrivedLot(int arrivedLotId, [FromBody] ArrivedLotEditDto dto)
        {
            var result = await _arrivedLotService.EditArrivedLotAsync(arrivedLotId, lot =>
            {
                lot.Quantity = dto.Quantity;
                lot.Grade = dto.Grade;
                lot.CropId = dto.CropId;
                lot.LotImageUrl = dto.LotImageUrl;
                lot.LotOwnerName = dto.LotOwnerName;
                lot.MobileNum = dto.MobileNum;
                lot.LotOwnerRole = dto.LotOwnerRole;
            });

            return result == null ? NotFound() : Ok(new ArrivedLotResponseDto(result));
        }

        // ----------------------------------------------------
        // 5. Update Status
        // ----------------------------------------------------
        [Authorize(Roles = "APPROVER")]
[HttpPatch("arrived/{arrivedLotId}/status")]
public async Task<IActionResult> UpdateStatus(int arrivedLotId,
    [FromBody] ArrivedLotStatusUpdateDto dto)
{
    // VALIDATION
    if (dto.NewStatus == "readyForAuction" && dto.AuctionId == null)
        return BadRequest("AuctionId is required when marking readyForAuction.");

    var lot = await _arrivedLotService.UpdateStatusAsync(
        arrivedLotId,
        dto.NewStatus,
        dto.AuctionId
    );

    if (lot == null) return NotFound();

    return Ok(new ArrivedLotResponseDto(lot));
}



        // ----------------------------------------------------
// 6. Get ArrivedLot Details
// ----------------------------------------------------
[HttpGet("arrived/{arrivedLotId}")]
public async Task<IActionResult> GetArrivedLotDetails(int arrivedLotId)
{
    var lot = await _db.ArrivedLots
        .Include(x => x.Farmer).ThenInclude(f => f.User)
        .Include(x => x.Seller).ThenInclude(s => s.User)
        .Include(x => x.Crop)
        .FirstOrDefaultAsync(x => x.ArrivedLotId == arrivedLotId);

    if (lot == null)
        return NotFound("Arrived lot not found.");

    // Auction info (if exists)
    var auction = await _db.LiveAuctionLots
        .FirstOrDefaultAsync(a => a.ArrivedLotId == arrivedLotId);

    return Ok(new ArrivedLotFullDetailDto(lot, auction));
}

    // -------------------------------------------------------------
    // 📌 DTOs (inside controller namespace)
    // -------------------------------------------------------------

    public class ArrivedLotCreateDto
    {
        public int MandiId { get; set; }
        public string LotOwnerRole { get; set; } = default!;
        public string LotOwnerName { get; set; } = default!;
        public string MobileNum { get; set; } = default!;
        public Guid? FarmerId { get; set; }
        public Guid? SellerId { get; set; }
        public string? PreLotId { get; set; }
        public int CropId { get; set; }
        public float Quantity { get; set; }
        public string? Grade { get; set; }
        public string? LotImageUrl { get; set; }
    }

    public class ArrivedLotEditDto
    {
        public float Quantity { get; set; }
        public string? Grade { get; set; }
        public int CropId { get; set; }
        public string? LotImageUrl { get; set; }
        public string LotOwnerName { get; set; } = default!;
        public string MobileNum { get; set; } = default!;
        public string LotOwnerRole { get; set; } = default!;
    }

    public class ArrivedLotResponseDto
    {
        public ArrivedLotResponseDto(ArrivedLot lot)
        {
            ArrivedLotId = lot.ArrivedLotId;
            LotOwnerName = lot.LotOwnerName;
            MobileNum = lot.MobileNum;
            LotOwnerRole = lot.LotOwnerRole;
            Status = lot.Status;
            QrCodeUrl = lot.QrCodeUrl;
            Quantity = lot.Quantity;
            Grade = lot.Grade;
            LotImageUrl = lot.LotImageUrl;
            CropId = lot.CropId;
            MandiId = lot.MandiId;
            CreatedAt = lot.CreatedAt;
        }

        public int ArrivedLotId { get; set; }
        public string LotOwnerName { get; set; }
        public string MobileNum { get; set; }
        public string LotOwnerRole { get; set; }
        public string Status { get; set; }
        public string QrCodeUrl { get; set; }
        public float Quantity { get; set; }
        public string? Grade { get; set; }
        public string? LotImageUrl { get; set; }
        public int CropId { get; set; }
        public int MandiId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class ArrivedLotFullDetailDto
{
    public ArrivedLotFullDetailDto(ArrivedLot lot, LiveAuctionLot? auction)
    {
        ArrivedLotId = lot.ArrivedLotId;
        Status = lot.Status;
        QrCodeUrl = lot.QrCodeUrl;

        // Owner
        LotOwnerRole = lot.LotOwnerRole;
        LotOwnerName = lot.LotOwnerName;
        MobileNum = lot.MobileNum;

        FarmerId = lot.FarmerId;
        SellerId = lot.SellerId;

        // Details
        CropName = lot.Crop?.CropName;
        Quantity = lot.Quantity;
        Grade = lot.Grade;
        LotImageUrl = lot.LotImageUrl;
        MandiId = lot.MandiId;
        CreatedAt = lot.CreatedAt;

        // Auction info
        if (auction != null)
        {
            AuctionStatus = auction.AuctionStatus;
            FinalPrice = auction.FinalPrice;
            BuyerId = auction.BuyerId;
            BuyerName = auction.BuyerName;
            BuyerMobile = auction.BuyerMobile;
        }
    }

    public int ArrivedLotId { get; set; }
    public string Status { get; set; }
    public string QrCodeUrl { get; set; }

    public string LotOwnerRole { get; set; }
    public string LotOwnerName { get; set; }
    public string MobileNum { get; set; }
    public Guid? FarmerId { get; set; }
    public Guid? SellerId { get; set; }

    public string? CropName { get; set; }
    public float Quantity { get; set; }
    public string? Grade { get; set; }
    public string? LotImageUrl { get; set; }
    public int MandiId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Auction Info
    public string? AuctionStatus { get; set; }
    public float? FinalPrice { get; set; }
    public Guid? BuyerId { get; set; }
    public string? BuyerName { get; set; }
    public string? BuyerMobile { get; set; }
}
public class ArrivedLotStatusUpdateDto
{
    public string NewStatus { get; set; } = default!;
    public Guid? AuctionId { get; set; }  // required for readyForAuction
}

    }

}
