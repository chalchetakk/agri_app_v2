using agriApp.Data;
using agriApp.Dtos.Lots;
using Microsoft.EntityFrameworkCore;

namespace agriApp.Services.Lots
{
    public class BuyerLotRecommendationService : IBuyerLotRecommendationService
    {
        private readonly AgriDbContext _db;

        public BuyerLotRecommendationService(AgriDbContext db)
        {
            _db = db;
        }

        public async Task<List<BuyerRecommendedLotDto>> GetRecommendedLotsForBuyerAsync(Guid buyerUserId)
{
    // 1️⃣ Resolve buyer
    var buyer = await _db.Buyers
        .AsNoTracking()
        .FirstOrDefaultAsync(b => b.UserId == buyerUserId);

    if (buyer == null)
        return new List<BuyerRecommendedLotDto>();

    // 2️⃣ Get PreLotIds already bid by this buyer
    var alreadyBidPreLotIds = await _db.BuyerInterestLots
        .Where(b => b.BuyerId == buyer.BuyerId)
        .Select(b => b.PreLotId)
        .ToListAsync();

    // 3️⃣ Fetch only NOT-YET-BID lots
    var lots = await _db.PreRegisteredLots
        .Include(l => l.Crop)
        .Include(l => l.Mandi)
        .Where(l =>
            l.Status == "preRegistered" &&
            !alreadyBidPreLotIds.Contains(l.PreLotId)
        )
        .OrderByDescending(l => l.CreatedAt)
        .ToListAsync();

    return lots.Select(l => new BuyerRecommendedLotDto
    {
        PreLotId = l.PreLotId,
        CropName = l.Crop?.CropName ?? "",
        Quantity = l.Quantity,
        Grade = l.Grade,
        LotImageUrl = l.LotImageUrl,
        Status = l.Status,
        MandiName = l.Mandi?.MandiName ?? "",
        MandiId = l.MandiId,
        ExpectedArrivalDate = l.ExpectedArrivalDate
    }).ToList();
}

    }
}
