// agriApp/Dtos/Lots/BuyerBidDetailDto.cs
namespace agriApp.Dtos.Lots
{
    public class BuyerBidDetailDto
    {
        public int BuyerInterestLotId { get; set; }
        public string PreLotId { get; set; } = default!;

        // Lot info
        public string CropName { get; set; } = default!;
        public float Quantity { get; set; }
        public string? Grade { get; set; }
        public string? LotImageUrl { get; set; }
        public string? QrCodeUrl { get; set; }
        public string LotStatus { get; set; } = default!;

        public int MandiId { get; set; }
        public string MandiName { get; set; } = default!;
        public DateTime ExpectedArrivalDate { get; set; }

        // Bid info
        public decimal BidAmount { get; set; }
        public string BidStatus { get; set; } = default!;
        public DateTime BidCreatedAt { get; set; }
    }
}
