using System;
using System.Collections.Generic;
using agriApp.Entities.Lots;

namespace agriApp.Dtos.Auctions
{
    public class AuctionDetailDto
    {
        public Guid AuctionId { get; set; }
        public int MandiId { get; set; }
        public string MandiName { get; set; } = "";

        public int CropId { get; set; }
        public string CropName { get; set; } = "";

        public Guid AssignedOfficerId { get; set; }
        public string AssignedOfficerName { get; set; } = "";

        public Guid CreatedByOfficialId { get; set; }
        public string CreatedByOfficialName { get; set; } = "";

        public string Status { get; set; } = "";
        public DateTime ScheduledAt { get; set; }
        public DateTime CreatedAt { get; set; }

        // Live Auction Lots
        public List<LiveAuctionLot> LiveLots { get; set; } = new();
    }
}
