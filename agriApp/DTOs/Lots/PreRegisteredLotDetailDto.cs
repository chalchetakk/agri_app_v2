namespace agriApp.Dtos.Lots
{
    public class PreRegisteredLotDetailDto
    {
        public string PreLotId { get; set; } = default!;
        public string Status { get; set; } = default!;

        // Crop + Mandi
        public int CropId { get; set; }
        public string CropName { get; set; } = default!;
        public int MandiId { get; set; }
        public string MandiName { get; set; } = default!;

        public string LotOwnerRole { get; set; } = "";   // FARMER / SELLER
public Guid LotOwnerId { get; set; }
public string LotOwnerName { get; set; } = "";
public string MobileNum { get; set; } = "";


        // Lot details
        public float Quantity { get; set; }
        public string Grade { get; set; } = default!;
        public float? SellingAmount { get; set; }
        public DateTime ExpectedArrivalDate { get; set; }

        // Media
        public string LotImageUrl { get; set; } = default!;
        public string? QrCodeUrl { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
