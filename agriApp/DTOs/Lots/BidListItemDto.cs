namespace agriApp.Dtos.Lots
{
    public class BidListItemDto
{
    public int BuyerInterestLotId { get; set; }
    public string BuyerName { get; set; } = default!;
    public string BuyerMobile { get; set; } = default!;
    public float BidAmount { get; set; }
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
}