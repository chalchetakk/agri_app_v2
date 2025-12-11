namespace agriApp.DTOs.Anchors
{
    public class AnchorFarmerListDto
    {
        public Guid FarmerId { get; set; }
        public string FarmerName { get; set; } = default!;
        public string MobileNumber { get; set; } = default!;
        public string? Location { get; set; }
        public int TotalFarms { get; set; }
        public List<string> InterestedCrops { get; set; } = new();
    }
}
