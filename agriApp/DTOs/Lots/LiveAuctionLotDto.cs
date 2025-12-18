namespace agriApp.Dtos.Lots
{
    public class LiveAuctionLotDto
    {
        public int LiveAuctionLotId { get; set; }
        public Guid? AuctionId { get; set; }
        public int ArrivedLotId { get; set; }

        public string? LotOwnerName { get; set; }
        public string? MobileNum { get; set; }
        public string? CropName { get; set; }
        public string? Grade { get; set; }
        public float Quantity { get; set; }
public float? FinalPrice { get; set; }

        public string? BuyerName { get; set; }
        public string? BuyerMobile { get; set; }

        public string AuctionStatus { get; set; } = "";
    }
}
