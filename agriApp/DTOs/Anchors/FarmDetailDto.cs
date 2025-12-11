namespace agriApp.DTOs.Anchors
{
    public class FarmDetailDto
    {
        public Guid FarmId { get; set; }
        public string FarmLocation { get; set; } = default!;
        public string PrimaryCrop { get; set; } = default!;
        public float FarmSize { get; set; }
    }
}
