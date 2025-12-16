namespace agriApp.Dtos.Buyers
{
    public class BuyerSearchResultDto
    {
        // BUYER | NOT_BUYER
        public string ResultType { get; set; } = "";

        public Guid? BuyerId { get; set; }
        public Guid? UserId { get; set; }

        public string Name { get; set; } = "";
        public string MobileNumber { get; set; } = "";

        // For NOT_BUYER cases
        public string? Message { get; set; }
    }
}
