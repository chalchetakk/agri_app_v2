using System;
using System.Collections.Generic;

namespace agriApp.DTOs.Anchors
{
    public class AnchorFarmerDetailDto
    {
        public Guid FarmerId { get; set; }
        public string FarmerName { get; set; } = default!;
        public string MobileNumber { get; set; } = default!;

        public string? ProfilePhotoUrl { get; set; }
        public string? Location { get; set; }

        // FIXED: Crop names
        public List<string> InterestedCrops { get; set; } = new();

        public List<FarmDetailDto> Farms { get; set; } = new();

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
