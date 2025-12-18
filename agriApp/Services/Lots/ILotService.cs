// agriApp/Services/Lots/ILotService.cs
using agriApp.Dtos.Lots;
using agriApp.Controllers;
namespace agriApp.Services.Lots
{
    public interface ILotService
    {
        Task<string> GeneratePreLotIdAsync();

        Task<LotListItemDto> RegisterLotAsync(Guid userId, LotRegisterRequest request, bool isFarmer);

        Task<List<LotListItemDto>> GetMyLotsAsync(Guid userId, bool isFarmer);

        Task<LotDetailDto?> GetLotByIdAsync(string preLotId, Guid userId, bool isFarmer);
        Task<LotDetailDto?> EditLotAsync(string preLotId, Guid userId, bool isFarmer, LotEditRequest request);

        // NEW: pre-registered delete
        Task<bool> DeleteLotAsync(string preLotId, Guid userId, bool isFarmer);

        // NEW: Auction / arrived view for owner
        Task<List<AuctionLotListItemDto>> GetMyAuctionLotsAsync(Guid userId, bool isFarmer);
        Task<AuctionLotDetailDto?> GetMyAuctionLotAsync(int arrivedLotId, Guid userId, bool isFarmer);
    // ============ Bidding (Farmer/Seller receives bids) ===============

// Get all bids for a specific lot (for farmer/seller)
Task<List<BidListItemDto>> GetBidsForLotAsync(string preLotId, Guid userId, bool isFarmer);

// Accept a bid
Task<bool> AcceptBidAsync(string preLotId, int buyerInterestLotId, Guid userId, bool isFarmer);

// Reject a bid
Task<bool> RejectBidAsync(string preLotId, int buyerInterestLotId, Guid userId, bool isFarmer);

// Get all bids received across all lots of farmer/seller
Task<List<ReceivedLotBidsDto>> GetAllReceivedBidsAsync(Guid userId, bool isFarmer);

    }
}
