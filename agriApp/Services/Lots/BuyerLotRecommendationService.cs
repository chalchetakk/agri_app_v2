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
            // For now show ALL preRegistered lots
            var lots = await _db.PreRegisteredLots
                .Include(l => l.Crop)
                .Include(l => l.Mandi)
                .Where(l => l.Status == "preRegistered")  // only available lots
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
