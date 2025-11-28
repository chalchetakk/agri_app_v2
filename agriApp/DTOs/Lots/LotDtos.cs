using Microsoft.AspNetCore.Http;

namespace agriApp.Dtos.Lots
{
    public class LotRegisterRequest
    {
        public int CropId { get; set; }
        public int MandiId { get; set; }

        public float Quantity { get; set; }
        public string Grade { get; set; } = default!;
        public DateTime ExpectedArrivalDate { get; set; }

        public float? SellingAmount { get; set; }

        public IFormFile? LotImage { get; set; }
    }

    public class LotListItemDto
    {
        public string PreLotId { get; set; } = default!;
        public string Status { get; set; } = default!;

        public string LotImageUrl { get; set; } = default!;
        public string QrCodeUrl { get; set; } = default!;

        public string CropName { get; set; } = default!;
        public string MandiName { get; set; } = default!;

        public float Quantity { get; set; }
        public string Grade { get; set; } = default!;
        public DateTime ExpectedArrivalDate { get; set; }

        public float? SellingAmount { get; set; }
    }

    public class LotDetailDto
    {
        public string PreLotId { get; set; } = default!;
        public string Status { get; set; } = default!;

        public int CropId { get; set; }
        public string CropName { get; set; } = default!;

        public int MandiId { get; set; }
        public string MandiName { get; set; } = default!;
        public string MandiLocation { get; set; } = default!;

        public float Quantity { get; set; }
        public string Grade { get; set; } = default!;
        public float? SellingAmount { get; set; }

        public DateTime ExpectedArrivalDate { get; set; }

        public string LotImageUrl { get; set; } = default!;
        public string QrCodeUrl { get; set; } = default!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
