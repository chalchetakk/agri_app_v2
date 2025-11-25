using System;
using agriApp.Entities.Stakeholders;
using agriApp.Entities.Market;

namespace agriApp.Entities.Stakeholders
{
    public class SellerInterestedCrop
    {
        public int SellerInterestedCropId { get; set; }

        public Guid SellerId { get; set; }
        public Seller? Seller { get; set; }

        public int CropId { get; set; }     // crop remains int
        public Crop? Crop { get; set; }
    }
}
