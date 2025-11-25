using System;
using agriApp.Entities.Stakeholders;

namespace agriApp.Entities.Stakeholders
{
    public class FarmDetails
    {
        public Guid FarmId { get; set; }

        public Guid FarmerId { get; set; }
        public Farmer? Farmer { get; set; }

        public string FarmLocation { get; set; } = default!;
        public string PrimaryCrop { get; set; } = default!;
        public float FarmSize { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public FarmDetails() { }

        public FarmDetails(Guid farmerId, string farmLocation, string primaryCrop, float farmSize)
        {
            FarmId = Guid.NewGuid();
            FarmerId = farmerId;
            FarmLocation = farmLocation;
            PrimaryCrop = primaryCrop;
            FarmSize = farmSize;
            CreatedAt = DateTime.UtcNow;
        }

        public void Touch() => UpdatedAt = DateTime.UtcNow;
    }
}
