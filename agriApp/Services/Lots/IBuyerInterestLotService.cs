using agriApp.Controllers;

namespace agriApp.Services.Lots
{
    public interface IBuyerInterestLotService
    {
        Task<List<BuyerLotsController.BuyerLotListItemDto>> GetLotsAvailableForBuyerAsync(Guid buyerUserId);
        Task<BuyerLotsController.BuyerLotDetailDto?> GetLotDetailForBuyerAsync(string preLotId, Guid buyerUserId);
        Task<BuyerLotsController.BuyerPlacedBidDto> PlaceBidAsync(Guid buyerUserId, string preLotId, decimal bidAmount);
        Task<List<BuyerLotsController.BuyerPlacedBidDto>> GetMyPlacedBidsAsync(Guid buyerUserId);
    }
}
