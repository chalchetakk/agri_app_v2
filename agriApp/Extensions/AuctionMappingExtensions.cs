using agriApp.Dtos.Auctions;
using agriApp.Dtos.Lots;
using agriApp.Entities.Auctions;
using agriApp.Entities.Lots;

namespace agriApp.Extensions
{
    public static class AuctionMappingExtensions
    {
        public static AuctionDetailDto ToDetailDto(
            this Auction auction,
            List<LiveAuctionLot> liveLots)
        {
            return new AuctionDetailDto
            {
                AuctionId = auction.AuctionId,
                MandiId = auction.MandiId,
                MandiName = auction.Mandi?.MandiName ?? "",
                CropId = auction.CropId,
                CropName = auction.Crop?.CropName ?? "",
                AssignedOfficerId = auction.AssignedOfficerId,
                AssignedOfficerName = auction.AssignedOfficer?.OfficialName ?? "",
                CreatedByOfficialId = auction.CreatedByOfficialId,
                CreatedByOfficialName = auction.CreatedByOfficial?.OfficialName ?? "",
                Status = auction.Status,
                ScheduledAt = auction.ScheduledAt,
                CreatedAt = auction.CreatedAt,

                LiveLots = liveLots.Select(l => new LiveAuctionLot
                {
                    LiveAuctionLotId = l.LiveAuctionLotId,
                    ArrivedLotId = l.ArrivedLotId,
                }).ToList()
            };
        }
    }
}
