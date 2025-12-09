using agriApp.Entities.Auctions;

namespace agriApp.Dtos.Auctions
{
    public static class AuctionDtoMapper
    {
        public static AuctionDetailDto ToDetailDto(Auction a)
        {
            return new AuctionDetailDto
            {
                AuctionId = a.AuctionId,

                MandiId = a.MandiId,
                MandiName = a.Mandi?.MandiName ?? "",

                CropId = a.CropId,
                CropName = a.Crop?.CropName ?? "",

                AssignedOfficerId = a.AssignedOfficerId,
                AssignedOfficerName = a.AssignedOfficer?.OfficialName ?? "",

                CreatedByOfficialId = a.CreatedByOfficialId,
                CreatedByOfficialName = a.CreatedByOfficial?.OfficialName ?? "",

                Status = a.Status,
                ScheduledAt = a.ScheduledAt,
                CreatedAt = a.CreatedAt,

                LiveLots = a.LiveAuctionLots?.ToList() ?? new()
            };
        }
    }
}
