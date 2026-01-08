using System.Collections.Generic;

namespace agriApp.Entities.Geography
{
    public class GeoDistrict
    {
        public long DistrictId { get; set; }   // PK

        public string DistrictName { get; set; } = default!;

        public string? DistrictCode { get; set; }

        public long StateId { get; set; }       // FK → GeoState

        public bool IsActive { get; set; } = true;

        public GeoState State { get; set; } = default!;

        public ICollection<GeoTaluka> Talukas { get; set; } = new List<GeoTaluka>();
    }
}
