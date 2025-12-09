namespace agriApp.DTOs.Lots
{
    public class UpdateLiveLotStatusRequest
    {
        public string Status { get; set; } = default!;   // "sold" or "unsold"
        public float? FinalPrice { get; set; }           // required if sold
        public Guid? BuyerId { get; set; }               // optional
        public string? BuyerName { get; set; }
        public string? BuyerMobile { get; set; }
    }
}
