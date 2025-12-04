namespace agriApp.Dtos.Lots
{
    public class ReceivedBidListItemDto
{
    public int BuyerInterestLotId { get; set; }
    public string PreLotId { get; set; } = default!;
    public float BidAmount { get; set; }
    public string Status { get; set; } = default!;
    public string BuyerName { get; set; } = default!;
    public string BuyerMobile { get; set; } = default!;
    public string CropName { get; set; } = default!;
    public string MandiName { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
}