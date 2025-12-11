namespace agriApp.DTOs.Anchors
{
    public class AnchorSingleFarmerResponseDto
    {
        public Guid FarmerId { get; set; }
        public Guid AnchorId { get; set; }
        public string Message { get; set; } = default!;
    }
}
