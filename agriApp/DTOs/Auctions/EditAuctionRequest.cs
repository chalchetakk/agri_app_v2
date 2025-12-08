using System;

namespace agriApp.Dtos.Auctions
{
    public class EditAuctionRequest
    {
        public int? CropId { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public Guid? AssignedOfficerId { get; set; }
    }
}
