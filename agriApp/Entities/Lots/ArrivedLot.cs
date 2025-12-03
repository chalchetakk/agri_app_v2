using System;
// using agriApp.Entities.Lots;
using agriApp.Entities.Market;
using agriApp.Entities.Stakeholders; 

namespace agriApp.Entities.Lots
{
    public class ArrivedLot
    {
        public int ArrivedLotId { get; set; }

        // Mandi Info
        public int MandiId { get; set; }
        public Mandi? Mandi { get; set; }

        // Owner Info (always stored here)
        public string LotOwnerRole { get; set; } = default!; // "farmer" or "seller"
        public string LotOwnerName { get; set; } = default!;
        public string MobileNum { get; set; } = default!;     // 10-digit mobile

        // Optional links to app users
        public Guid? FarmerId { get; set; }
        public Farmer? Farmer { get; set; }

        public Guid? SellerId { get; set; }
        public Seller? Seller { get; set; }

        // Optional link to pre-registered lot
        public string? PreLotId { get; set; }
        public PreRegisteredLot? PreRegisteredLot { get; set; }

        // Gate Officer
        public Guid MandiOfficerId { get; set; }
        public MandiOfficial? MandiOfficer { get; set; }

        // Lot Details
        public int CropId { get; set; }
        public Crop? Crop { get; set; }

        public float Quantity { get; set; }
        public string? Grade { get; set; }  // optional

        public string? LotImageUrl { get; set; } // optional
        public string QrCodeUrl { get; set; } = default!;  // REQUIRED

        // Status: arrived, verified, readyForAuction
        public string Status { get; set; } = default!;

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
