namespace agriApp.Dtos.Lots
{
    public class ArrivedLotDetailDto
    {
        public int ArrivedLotId { get; set; }
        public string? PreLotId { get; set; }

        public int CropId { get; set; }
        public string CropName { get; set; } = default!;

        public int MandiId { get; set; }
        public string MandiName { get; set; } = default!;

        public string Status { get; set; } = default!;

        public float Quantity { get; set; }
        public string? Grade { get; set; }

        public string? LotImageUrl { get; set; }
        public string? QrCodeUrl { get; set; }

        public DateTime? ExpectedArrivalDate { get; set; }
        public float? SellingAmount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
