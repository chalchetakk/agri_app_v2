using System;
using agriApp.Entities.Stakeholders;

namespace agriApp.Entities.Lots
{
    public class BuyerInterestLot
    {
        public int BuyerInterestLotId { get; set; } // PK: serial

        // FK → PreRegisteredLots
        public string PreLotId { get; set; } = default!;
        public PreRegisteredLot? PreRegisteredLot { get; set; }

        // FK → Buyers table (GUID)
        public Guid BuyerId { get; set; }
        public Buyer? Buyer { get; set; }

        // Bid amount
        public decimal BuyerBidAmount { get; set; }

        // Status: pending / accepted / rejected
        public string Status { get; set; } = "pending";

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
