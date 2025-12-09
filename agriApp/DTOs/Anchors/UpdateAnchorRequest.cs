namespace agriApp.DTOs.Anchors
{
    public class UpdateAnchorRequest
    {
        public string? CompanyName { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? CompanyAddress { get; set; }
        public string? ContactPersonName { get; set; }
        public string? Email { get; set; }
        public string? ContactPersonNum { get; set; }

        public string? GSTNumber { get; set; }

        public int? EstimatedFarmersNum { get; set; }

        public string? BusinessDescription { get; set; }
    }
}
