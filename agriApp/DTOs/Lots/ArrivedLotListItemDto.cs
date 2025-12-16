namespace agriApp.Dtos.Lots
{
    public class ArrivedLotListItemDto
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
// 👤 Lot owner snapshot (NEW)
        public string LotOwnerName { get; set; } = default!;
        public string MobileNum { get; set; } = default!;
        public string LotOwnerRole { get; set; } = default!; // FARMER / SELLER
        public string? LotImageUrl { get; set; }
        public string? QrCodeUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
