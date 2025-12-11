namespace agriApp.DTOs.Anchors
{
    public class BulkFarmerRowDto
    {
        public int RowNumber { get; set; }
        public string FarmerName { get; set; } = default!;
        public string Mobile { get; set; } = default!;
        public string? Location { get; set; }
        public string? ProfilePhotoUrl { get; set; }

        public List<string> InterestedCrops { get; set; } = new();

        public string FarmLocation { get; set; } = default!;
        public string PrimaryCrop { get; set; } = default!;
        public float FarmSize { get; set; }
    }
}
