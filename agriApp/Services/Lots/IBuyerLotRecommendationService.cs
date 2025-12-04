using agriApp.Dtos.Lots;

namespace agriApp.Services.Lots
{
    public interface IBuyerLotRecommendationService
    {
        // For now show all preRegistered lots
        Task<List<BuyerRecommendedLotDto>> GetRecommendedLotsForBuyerAsync(Guid buyerUserId);
    }
}
