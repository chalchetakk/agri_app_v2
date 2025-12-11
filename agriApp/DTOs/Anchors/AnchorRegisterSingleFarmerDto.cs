namespace agriApp.DTOs.Anchors
{
    public class AnchorRegisterSingleFarmerDto
    {
        public string FarmerName { get; set; } = default!;
        public string MobileNumber { get; set; } = default!;
        public string? Location { get; set; }
        public string? ProfilePhotoUrl { get; set; }

        // List of crop names (e.g., ["Onion","Wheat"])
        public List<string> InterestedCrops { get; set; } = new();

        public List<SingleFarmInputDto> Farms { get; set; } = new();
    }

    public class SingleFarmInputDto
    {
        public string FarmLocation { get; set; } = default!;
        public string PrimaryCrop { get; set; } = default!;
        public float FarmSize { get; set; }
    }
}
