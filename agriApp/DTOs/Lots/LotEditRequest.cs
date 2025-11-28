using Microsoft.AspNetCore.Http;

namespace agriApp.Dtos.Lots
{
    public class LotEditRequest
    {
        public int CropId { get; set; }
        public int MandiId { get; set; }

        public float Quantity { get; set; }
        public string Grade { get; set; } = default!;

        public float SellingAmount { get; set; }
        public DateTime ExpectedArrivalDate { get; set; }

        // Optional new image
        public IFormFile? LotImage { get; set; }
    }
}
