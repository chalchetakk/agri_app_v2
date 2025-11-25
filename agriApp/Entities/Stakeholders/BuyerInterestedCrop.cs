using System;

namespace agriApp.Entities.Stakeholders
{
    public class BuyerInterestedCrop
    {
        public Guid BuyerInterestedCropId { get; set; }

        public Guid BuyerId { get; set; }
        public Buyer? Buyer { get; set; }

        public int CropId { get; set; }  // ref to crop table

        public BuyerInterestedCrop() {}

        public BuyerInterestedCrop(Guid buyerId, int cropId)
        {
            BuyerInterestedCropId = Guid.NewGuid();
            BuyerId = buyerId;
            CropId = cropId;
        }
    }
}
