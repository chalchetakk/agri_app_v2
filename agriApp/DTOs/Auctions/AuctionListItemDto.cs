using System;

namespace agriApp.Dtos.Auctions
{
    public class AuctionListItemDto
    {
        public Guid AuctionId { get; set; }
        public int MandiId { get; set; }
        public int CropId { get; set; }
        public string CropName { get; set; } = "";
        public string MandiName { get; set; } = "";
        public string AssignedOfficerName { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime ScheduledAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
