namespace agriApp.DTOs.Anchors
{
    public class TemplateDownloadDto
    {
        public Stream FileStream { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public string ContentType { get; set; } = default!;
    }
}
