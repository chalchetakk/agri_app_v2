namespace agriApp.Dtos.Auctions
{
    public class UpdateLiveLotStatusRequest
    {
        public string Status { get; set; } = "";  // "sold" or "unsold"
        public float? FinalPrice { get; set; }     // Required only if sold

        public Guid? BuyerId { get; set; }         // Optional if sold
        public string? BuyerName { get; set; }     // Required if BuyerId is null
        public string? BuyerMobile { get; set; }   // Required if BuyerId is null
    }
}
