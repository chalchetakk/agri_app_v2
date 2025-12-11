namespace agriApp.DTOs.Anchors
{
    public class BulkRowErrorDto
    {
        public int RowNumber { get; set; }
        public string Column { get; set; } = default!;
        public string ErrorMessage { get; set; } = default!;
    }
}
