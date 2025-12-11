using agriApp.DTOs.Anchors;
using Microsoft.AspNetCore.Http;

namespace agriApp.Services.BulkImport
{
    public interface IBulkFileParserService
    {
        Task<List<BulkFarmerRowDto>> ParseAsync(IFormFile file);
    }
}
