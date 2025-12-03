using agriApp.Entities.Lots;
using System;

namespace agriApp.Services.Lots
{
    public interface ILiveAuctionLotService
    {
        Task<LiveAuctionLot> CreateAuctionEntryAsync(int arrivedLotId);
        Task<LiveAuctionLot?> MarkSoldAsync(
            int liveAuctionLotId,
            float finalPrice,
            Guid? buyerId,
            string? buyerName,
            string? buyerMobile);

        Task<LiveAuctionLot?> MarkUnsoldAsync(int liveAuctionLotId);
    }
}
