namespace agriApp.Dtos.Lots
{
    public class ArrivedLotDetailDto
    {
        public int ArrivedLotId { get; set; }
        public string? PreLotId { get; set; }

        // 🌾 Crop info
        public int CropId { get; set; }
        public string CropName { get; set; } = default!;

        // 🏪 Mandi info
        public int MandiId { get; set; }
        public string MandiName { get; set; } = default!;

        // 📦 Lot status
        public string Status { get; set; } = default!;

        // 📊 Lot details
        public float Quantity { get; set; }
        public string? Grade { get; set; }

        // 👤 Lot owner snapshot (NEW)
        public string LotOwnerName { get; set; } = default!;
        public string MobileNum { get; set; } = default!;
        public string LotOwnerRole { get; set; } = default!; // FARMER / SELLER

        // 🖼 Media
        public string? LotImageUrl { get; set; }
        public string? QrCodeUrl { get; set; }

        // 📅 Pre-registered lot info (optional)
        public DateTime? ExpectedArrivalDate { get; set; }
        public float? SellingAmount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
