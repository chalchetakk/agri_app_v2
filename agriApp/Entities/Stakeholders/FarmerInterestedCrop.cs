using System;
using agriApp.Entities.Stakeholders;

namespace agriApp.Entities.Stakeholders
{
    public class FarmerInterestedCrop
    {
        public Guid FarmerInterestedCropId { get; set; }

        public Guid FarmerId { get; set; }
        public Farmer? Farmer { get; set; }

        public int CropId { get; set; }   // FK -> Crops table

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public FarmerInterestedCrop() { }

        public FarmerInterestedCrop(Guid farmerId, int cropId)
        {
            FarmerInterestedCropId = Guid.NewGuid();
            FarmerId = farmerId;
            CropId = cropId;
            CreatedAt = DateTime.UtcNow;
        }

        public void Touch() => UpdatedAt = DateTime.UtcNow;
    }
}
