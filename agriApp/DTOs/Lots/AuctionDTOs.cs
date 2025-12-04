// agriApp/Dtos/Lots/AuctionDtos.cs
namespace agriApp.Dtos.Lots
{
    public class AuctionLotListItemDto
    {
        public int ArrivedLotId { get; set; }
        public string? PreLotId { get; set; }           // if pre-registered
        public string CropName { get; set; } = default!;
        public int CropId { get; set; }
        public float Quantity { get; set; }
        public string? Grade { get; set; }
        public string? LotImageUrl { get; set; }
        public string? QrCodeUrl { get; set; }
        public string Status { get; set; } = default!;  // arrived / verified / readyForAuction / sold / unsold
        public DateTime CreatedAt { get; set; }
        public int MandiId { get; set; }
        public string? MandiName { get; set; }
        // Auction info (if created)
        public int? LiveAuctionLotId { get; set; }
        public string? AuctionStatus { get; set; }      // pending / sold / unsold
        public float? FinalPrice { get; set; }
        public string? BuyerName { get; set; }
        public string? BuyerMobile { get; set; }
    }

    public class AuctionLotDetailDto
    {
        public int ArrivedLotId { get; set; }
        public string? PreLotId { get; set; }           
        public int MandiId { get; set; }
        public string? MandiName { get; set; }
        public int CropId { get; set; }
        public string CropName { get; set; } = default!;
        public float Quantity { get; set; }
        public string? Grade { get; set; }
        public string? LotImageUrl { get; set; }
        public string? QrCodeUrl { get; set; }
        public string Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Pre-registered fields
        public DateTime? ExpectedArrivalDate { get; set; }
        public float? SellingAmount { get; set; }

        // Auction details
        public int? LiveAuctionLotId { get; set; }
        public string? AuctionStatus { get; set; }
        public float? FinalPrice { get; set; }
        public string? BuyerName { get; set; }
        public string? BuyerMobile { get; set; }
    }
}
