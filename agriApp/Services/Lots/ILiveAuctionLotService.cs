using agriApp.Entities.Lots;
using System;
using agriApp.Dtos.Lots;
namespace agriApp.Services.Lots
{
    public interface ILiveAuctionLotService
    {
        Task<LiveAuctionLot> CreateAuctionEntryAsync(int arrivedLotId);
        Task<LiveAuctionLotDto?> MarkSoldAsync(
            int liveAuctionLotId,
            float finalPrice,
            Guid? buyerId,
            string? buyerName,
            string? buyerMobile);

        Task<LiveAuctionLotDto?> MarkUnsoldAsync(int liveAuctionLotId);
        Task<LiveAuctionLotDto?> GetByIdAsync(int liveLotId);

    }
}
