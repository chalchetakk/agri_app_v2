namespace agriApp.Dtos.Lots
{
    public class PreRegisteredLotListItemDto
    {
        public string PreLotId { get; set; } = default!;
        public int CropId { get; set; }
        public string CropName { get; set; } = default!;
        public int MandiId { get; set; }
        public string MandiName { get; set; } = default!;
        public string Status { get; set; } = default!;
public string LotOwnerRole { get; set; } = "";   // FARMER / SELLER
public string LotOwnerName { get; set; } = "";
public string MobileNum { get; set; } = "";

        public float Quantity { get; set; }
        public string Grade { get; set; } = default!;

        public string LotImageUrl { get; set; } = default!;
        public string? QrCodeUrl { get; set; }

        public DateTime ExpectedArrivalDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
