using System;
using agriApp.Entities.Stakeholders;

namespace agriApp.Entities.Stakeholders
{
    public class AnchorFarmer
    {
        public Guid AnchorFarmerId { get; set; }

        public Guid AnchorId { get; set; }
        public Anchor? Anchor { get; set; }

        public Guid FarmerId { get; set; }
        public Farmer? Farmer { get; set; }

        public DateTime CreatedAt { get; set; }

        public AnchorFarmer() {}

        public AnchorFarmer(Guid anchorId, Guid farmerId)
        {
            AnchorFarmerId = Guid.NewGuid();
            AnchorId = anchorId;
            FarmerId = farmerId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
