namespace agriApp.DTOs.Anchors
{
    public class BulkRegisterResultDto
    {
        public int TotalRows { get; set; }
        public int TotalFarmersCreated { get; set; }
        public int TotalFarmsCreated { get; set; }

        public List<BulkRowErrorDto> Errors { get; set; } = new();
    }
}
