using System;
using agriApp.Entities.Auctions;

namespace agriApp.Dtos.Auctions
{
    public class CreateAuctionRequest
    {
        public int MandiId { get; set; }
        public int CropId { get; set; }
        public DateTime ScheduledAt { get; set; }

        // Required officer assignment
        public Guid AssignedOfficerId { get; set; }

        // Manager creating auction (populated by controller from JWT)
        public Guid CreatedByOfficialId { get; set; }

        public Auction ToEntity()
        {
            return new Auction
            {
                AuctionId = Guid.NewGuid(),
                MandiId = this.MandiId,
                CropId = this.CropId,
                ScheduledAt = this.ScheduledAt,
                AssignedOfficerId = this.AssignedOfficerId,
                CreatedByOfficialId = this.CreatedByOfficialId,

                Status = "scheduled",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
