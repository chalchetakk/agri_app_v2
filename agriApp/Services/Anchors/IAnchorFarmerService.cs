using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using agriApp.DTOs.Anchors;

namespace agriApp.Services.Anchors
{
    public interface IAnchorFarmerService
    {
        // 1️⃣ Register a single farmer
        Task<AnchorSingleFarmerResponseDto> RegisterSingleFarmerAsync(
            Guid anchorId,
            AnchorRegisterSingleFarmerDto dto);

        // 2️⃣ Bulk registration from CSV/Excel
        Task<BulkRegisterResultDto> RegisterBulkFarmersAsync(
            Guid anchorId,
            IFormFile file);

        // 3️⃣ List all farmers under this anchor
        Task<List<AnchorFarmerListDto>> GetFarmersAsync(Guid anchorId);

        // 4️⃣ Fetch full details of a farmer
        Task<AnchorFarmerDetailDto> GetFarmerDetailAsync(
            Guid anchorId,
            Guid farmerId);

        // 5️⃣ Templates (MemoryStreams for download)
        Task<MemoryStream> GenerateCsvTemplateAsync();
        Task<MemoryStream> GenerateExcelTemplateAsync();
    }
}
