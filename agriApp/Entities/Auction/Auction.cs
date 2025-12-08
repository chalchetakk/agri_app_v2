using System;
using System.Collections.Generic;
using agriApp.Entities.Market;
using agriApp.Entities.Stakeholders;
using agriApp.Entities.Lots;

namespace agriApp.Entities.Auctions
{
    public class Auction
    {
        // Primary Key (GUID)
        public Guid AuctionId { get; set; }

        // FK → Mandi
        public int MandiId { get; set; }
        public Mandi? Mandi { get; set; }

        // FK → Crop
        public int CropId { get; set; }
        public Crop? Crop { get; set; }

        // FK → MandiOfficial (Assigned Officer)
        public Guid AssignedOfficerId { get; set; }
        public MandiOfficial? AssignedOfficer { get; set; }

        // FK → MandiOfficial (Manager who created this auction)
        public Guid CreatedByOfficialId { get; set; }
        public MandiOfficial? CreatedByOfficial { get; set; }

        // Auction parameters
        public DateTime ScheduledAt { get; set; }

        // scheduled | started | ended | cancelled
        public string Status { get; set; } = default!;

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation: Lots assigned to this auction
        public List<LiveAuctionLot> LiveAuctionLots { get; set; } = new();
    }
}
