namespace agriApp.Entities.Geography
{
    public class GeoTaluka
    {
        public long TalukaId { get; set; }   // PK

        public string TalukaName { get; set; } = default!;

        public string? TalukaCode { get; set; }

        public long DistrictId { get; set; }  // FK → GeoDistrict

        public bool IsActive { get; set; } = true;

        public GeoDistrict District { get; set; } = default!;
    }
}
