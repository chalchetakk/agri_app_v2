using agriApp.Data;
using agriApp.Dtos.Lots;
using Microsoft.EntityFrameworkCore;
using agriApp.Dtos.Lots;

namespace agriApp.Services.Lots
{
    public interface IPreRegisteredLotQueryService
    {
        Task<List<PreRegisteredLotListItemDto>> GetLotsForMandiAsync(int mandiId);
        Task<PreRegisteredLotDetailDto?> GetLotByIdAsync(string preLotId);
    }

    public class PreRegisteredLotQueryService : IPreRegisteredLotQueryService
    {
        private readonly AgriDbContext _db;

        public PreRegisteredLotQueryService(AgriDbContext db)
        {
            _db = db;
        }

        // LIST FOR MANDI
        public async Task<List<PreRegisteredLotListItemDto>> GetLotsForMandiAsync(int mandiId)
        {
            return await _db.PreRegisteredLots
                .Include(l => l.Crop)
                .Include(l => l.Mandi)
                .Include(l => l.Farmer)
    .ThenInclude(f => f.User)
.Include(l => l.Seller)
    .ThenInclude(s => s.User)

                .Where(l => l.MandiId == mandiId)
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => new PreRegisteredLotListItemDto
                {
                    PreLotId = l.PreLotId,
                    CropId = l.CropId,
                    CropName = l.Crop!.CropName,
                    LotOwnerRole = l.FarmerId != null ? "FARMER" : "SELLER",
LotOwnerName = l.FarmerId != null
    ? l.Farmer!.FarmerName
    : l.Seller!.SellerName,
MobileNum = l.FarmerId != null
    ? l.Farmer!.User!.MobileNumber
    : l.Seller!.User!.MobileNumber,

                    MandiId = l.MandiId,
                    MandiName = l.Mandi!.MandiName,
                    Status = l.Status,
                    Quantity = l.Quantity,
                    Grade = l.Grade,
                    LotImageUrl = l.LotImageUrl,
                    QrCodeUrl = l.QrCodeUrl,
                    ExpectedArrivalDate = l.ExpectedArrivalDate,
                    CreatedAt = l.CreatedAt
                })
                .ToListAsync();
        }

        // SINGLE LOT
        public async Task<PreRegisteredLotDetailDto?> GetLotByIdAsync(string preLotId)
        {
            var lot = await _db.PreRegisteredLots
                .Include(l => l.Crop)
                .Include(l => l.Mandi)
                .Include(l => l.Farmer)
                .ThenInclude(f => f.User)
                .Include(l => l.Seller)
                .ThenInclude(s => s.User)   
                .FirstOrDefaultAsync(l => l.PreLotId == preLotId);

            if (lot == null) return null;
string ownerRole = "";
Guid ownerId = Guid.Empty;
string ownerName = "";
string mobile = "";
if (lot.FarmerId != null && lot.Farmer != null)
{
    ownerRole = "FARMER";
    ownerId = lot.Farmer.FarmerId;
    ownerName = lot.Farmer.FarmerName;
    mobile = lot.Farmer.User?.MobileNumber ?? "";
}
else if (lot.SellerId != null && lot.Seller != null)
{
    ownerRole = "SELLER";
    ownerId = lot.Seller.SellerId;
    ownerName = lot.Seller.SellerName;
    mobile = lot.Seller.User?.MobileNumber ?? "";
}

            return new PreRegisteredLotDetailDto
            {
                PreLotId = lot.PreLotId,
                Status = lot.Status,
                CropId = lot.CropId,
                CropName = lot.Crop!.CropName,

                LotOwnerRole = ownerRole,
LotOwnerId = ownerId,
LotOwnerName = ownerName,
MobileNum = mobile,

                MandiId = lot.MandiId,
                MandiName = lot.Mandi!.MandiName,
                Quantity = lot.Quantity,
                Grade = lot.Grade,
                SellingAmount = lot.SellingAmount,
                ExpectedArrivalDate = lot.ExpectedArrivalDate,
                LotImageUrl = lot.LotImageUrl,
                QrCodeUrl = lot.QrCodeUrl,
                CreatedAt = lot.CreatedAt,
                UpdatedAt = lot.UpdatedAt
            };
        }
    }
}
