using System;
using agriApp.Entities.Stakeholders;
using agriApp.Entities.Auctions;

namespace agriApp.Entities.Lots
{
    public class LiveAuctionLot
    {
        public int LiveAuctionLotId { get; set; }

        // Link to ArrivedLots (mandatory)
        public int ArrivedLotId { get; set; }
        public ArrivedLot? ArrivedLot { get; set; }

// ⭐ NEW — Link to Auction
        public Guid? AuctionId { get; set; }
        public Auction? Auction { get; set; }

        // Auction Status: pending, sold, unsold
        public string AuctionStatus { get; set; } = default!;

        // Price (only when sold)
        public float? FinalPrice { get; set; }

        // App-registered buyer (optional)
        public Guid? BuyerId { get; set; }
        public Buyer? Buyer { get; set; }

        // Offline buyer data (required if buyerId == null for sold)
        public string? BuyerName { get; set; }
        public string? BuyerMobile { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
