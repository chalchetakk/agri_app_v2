using System.Collections.Generic;

namespace agriApp.Entities.Geography
{
    public class GeoState
    {
        public long StateId { get; set; }   // PK

        public string StateName { get; set; } = default!;

        public string? StateCode { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<GeoDistrict> Districts { get; set; } = new List<GeoDistrict>();
    }
}
