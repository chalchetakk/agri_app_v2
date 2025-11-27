using System;
using agriApp.Entities.Stakeholders;
using agriApp.Entities.Market;


namespace agriApp.Entities.Lots
{
    public class PreRegisteredLot
    {
        // 10-digit string ID (not Guid)
        public string PreLotId { get; set; } = default!;

        // Owner — Either Farmer OR Seller
        public Guid? FarmerId { get; set; }
        public Farmer? Farmer { get; set; }

        public Guid? SellerId { get; set; }
        public Seller? Seller { get; set; }

        // Required foreign keys
        public int CropId { get; set; }
        public Crop? Crop { get; set; }

        public int MandiId { get; set; }
        public Mandi? Mandi { get; set; }

        // Lot Details
        public string Status { get; set; } = default!;   // preRegistered, arrived, verified, readyForAuction, sold, unsold
        public float Quantity { get; set; }
        public string Grade { get; set; } = default!;
        public float? SellingAmount { get; set; }

        public string LotImageUrl { get; set; } = default!;
        public string QrCodeUrl { get; set; } = default!;

        public DateTime ExpectedArrivalDate { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
