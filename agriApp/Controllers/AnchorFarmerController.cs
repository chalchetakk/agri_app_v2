using agriApp.DTOs.Anchors;
using agriApp.Services.Anchors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using agriApp.Data;
using Microsoft.EntityFrameworkCore;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("anchor")]
    [Authorize]
    public class AnchorFarmerController : ControllerBase
    {
        private readonly IAnchorFarmerService _anchorFarmerService;
        private readonly AgriDbContext _db;

        public AnchorFarmerController(
            IAnchorFarmerService anchorFarmerService,
            AgriDbContext db)
        {
            _anchorFarmerService = anchorFarmerService;
            _db = db;
        }

        // --------------------------------------------------------------------
        // Helper: Resolve AnchorId from logged-in user's JWT
        // --------------------------------------------------------------------
        private async Task<Guid> GetAnchorIdFromUserAsync()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
                throw new Exception("Invalid token. UserId missing.");

            var userId = Guid.Parse(userIdClaim);

            var anchor = await _db.Anchors
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (anchor == null)
                throw new Exception("User is not registered as an anchor.");

            return anchor.AnchorId;
        }

        // --------------------------------------------------------------------
        // 1️⃣ REGISTER SINGLE FARMER
        // POST /anchor/farmer/register-single
        // --------------------------------------------------------------------
        [HttpPost("farmer/register-single")]
        public async Task<IActionResult> RegisterSingleFarmer(
            [FromBody] AnchorRegisterSingleFarmerDto dto)
        {
            try
            {
                var anchorId = await GetAnchorIdFromUserAsync();

                var result = await _anchorFarmerService.RegisterSingleFarmerAsync(anchorId, dto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // --------------------------------------------------------------------
        // 2️⃣ BULK REGISTER FARMERS
        // POST /anchor/farmers/register-bulk
        // Content-Type: multipart/form-data
        // --------------------------------------------------------------------
        [HttpPost("farmers/register-bulk")]
        public async Task<IActionResult> RegisterBulkFarmers([FromForm] BulkUploadRequestDto request)
        {
            if (request.File == null)
                return BadRequest(new { message = "Bulk import file is required." });

            try
            {
                var anchorId = await GetAnchorIdFromUserAsync();

                var result = await _anchorFarmerService.RegisterBulkFarmersAsync(anchorId, request.File);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // --------------------------------------------------------------------
        // 3️⃣ GET FARMERS LIST
        // GET /anchor/farmers
        // --------------------------------------------------------------------
        [HttpGet("farmers")]
        public async Task<IActionResult> GetFarmers()
        {
            try
            {
                var anchorId = await GetAnchorIdFromUserAsync();

                var farmers = await _anchorFarmerService.GetFarmersAsync(anchorId);

                return Ok(farmers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // --------------------------------------------------------------------
        // 4️⃣ GET SINGLE FARMER DETAIL
        // GET /anchor/farmers/{farmerId}
        // --------------------------------------------------------------------
        [HttpGet("farmers/{farmerId}")]
        public async Task<IActionResult> GetFarmerDetail(Guid farmerId)
        {
            try
            {
                var anchorId = await GetAnchorIdFromUserAsync();

                var detail = await _anchorFarmerService.GetFarmerDetailAsync(anchorId, farmerId);

                return Ok(detail);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // --------------------------------------------------------------------
        // 5️⃣ DOWNLOAD CSV TEMPLATE
        // GET /anchor/download-template/csv
        // --------------------------------------------------------------------
        [HttpGet("download-template/csv")]
        public async Task<IActionResult> DownloadCsvTemplate()
        {
            var stream = await _anchorFarmerService.GenerateCsvTemplateAsync();
            stream.Position = 0;

            return File(
                stream,
                "text/csv",
                "anchor_farmers_template.csv"
            );
        }

        // --------------------------------------------------------------------
        // 6️⃣ DOWNLOAD EXCEL TEMPLATE
        // GET /anchor/download-template/excel
        // --------------------------------------------------------------------
        [HttpGet("download-template/excel")]
        public async Task<IActionResult> DownloadExcelTemplate()
        {
            var stream = await _anchorFarmerService.GenerateExcelTemplateAsync();
            stream.Position = 0;

            return File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "anchor_farmers_template.xlsx"
            );
        }
    }
}
