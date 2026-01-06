namespace agriApp.Dtos.Lots
{
    public class BuyerRecommendedLotDto
    {
        public string PreLotId { get; set; } = default!;
        public string CropName { get; set; } = default!;
        public float Quantity { get; set; }
        public string? Grade { get; set; }
        public string? LotImageUrl { get; set; }
        public string Status { get; set; } = default!;
        public string MandiName { get; set; } = default!;
        public int MandiId { get; set; }
        public DateTime ExpectedArrivalDate { get; set; }

        public float? SellingAmount { get; set; } 
    }
}
