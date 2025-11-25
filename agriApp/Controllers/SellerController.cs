using agriApp.Services.Sellers;
using agriApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("seller")]
    [Authorize]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _sellerService;
        private readonly AgriDbContext _db;

        public SellerController(ISellerService sellerService, AgriDbContext db)
        {
            _sellerService = sellerService;
            _db = db;
        }

        // -------------------------------------------------------
        // POST /seller/register
        // -------------------------------------------------------
        [HttpPost("register")]
        public async Task<IActionResult> RegisterSeller([FromBody] SellerRegisterRequest request)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            try
            {
                await _sellerService.RegisterSellerAsync(userId, request);
                return Ok(new { message = "Seller registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // -------------------------------------------------------
        // GET /seller/status
        // -------------------------------------------------------
        [HttpGet("status")]
        public async Task<IActionResult> SellerStatus()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            var exists = await _sellerService.IsSellerExistsAsync(userId);

            return Ok(new { isSeller = exists });
        }

        // -------------------------------------------------------
        // GET /seller/profile
        // -------------------------------------------------------
        [HttpGet("profile")]
        public async Task<IActionResult> GetSellerProfile()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            var seller = await _db.Sellers
                        .Include(s => s.InterestedCrops)
                        .FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
                return NotFound(new { message = "Seller profile not found." });

            return Ok(new
            {
                sellerId = seller.SellerId,
                sellerName = seller.SellerName,
                businessName = seller.BusinessName,
                location = seller.Location,
                profilePhotoUrl = seller.ProfilePhotoUrl,
                interestedCropIds = seller.InterestedCrops.Select(c => c.CropId),
                createdAt = seller.CreatedAt,
                updatedAt = seller.UpdatedAt
            });
        }
    }

    // -------------------------------------------------------
    // DTOs
    // -------------------------------------------------------
    public class SellerRegisterRequest
    {
        public string SellerName { get; set; } = default!;
        public string? BusinessName { get; set; }
        public string Location { get; set; } = default!;
        public string? ProfilePhotoUrl { get; set; }
        public List<int> CropIds { get; set; } = new();
    }
}
