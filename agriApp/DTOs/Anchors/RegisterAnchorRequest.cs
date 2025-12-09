namespace agriApp.DTOs.Anchors
{
    public class RegisterAnchorRequest
    {
        public string CompanyName { get; set; } = default!;
        public string RegistrationNumber { get; set; } = default!;
        public string CompanyAddress { get; set; } = default!;
        public string ContactPersonName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string ContactPersonNum { get; set; } = default!;

        public string? GSTNumber { get; set; }

        public int EstimatedFarmersNum { get; set; }

        public string? BusinessDescription { get; set; }
    }
}
