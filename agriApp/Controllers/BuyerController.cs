using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using agriApp.Services.Buyers;
using Microsoft.EntityFrameworkCore;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("buyer")]
    [Authorize]
    public class BuyerController : ControllerBase
    {
        private readonly IBuyerService _buyerService;

        public BuyerController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }

        // ----------------------------------------------------
        // 1️⃣ CHECK BUYER STATUS
        // ----------------------------------------------------
        [HttpGet("status")]
        public async Task<IActionResult> GetBuyerStatus()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null)
                return Unauthorized(new { message = "Invalid user." });

            var userId = Guid.Parse(userIdClaim);

            var isBuyer = await _buyerService.IsBuyerAsync(userId);

            return Ok(new { isBuyer });
        }

        // ----------------------------------------------------
        // 2️⃣ REGISTER BUYER
        // ----------------------------------------------------
        [HttpPost("register")]
        public async Task<IActionResult> RegisterBuyer([FromBody] BuyerRegisterRequest request)
        {
            if (request == null)
                return BadRequest(new { message = "Invalid request." });

            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null)
                return Unauthorized(new { message = "Invalid user." });

            var userId = Guid.Parse(userIdClaim);

            try
            {
                var result = await _buyerService.RegisterBuyerAsync(userId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ----------------------------------------------------
        // 3️⃣ GET BUYER PROFILE
        // ----------------------------------------------------
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null)
                return Unauthorized(new { message = "Invalid user." });

            var userId = Guid.Parse(userIdClaim);

            var profile = await _buyerService.GetBuyerProfileAsync(userId);
            if (profile == null)
                return NotFound(new { message = "Buyer data not found." });

            return Ok(profile);
        }
    }

    // --------------------------------------------
    // DTOs inside controller (temporary placement)
    // --------------------------------------------
    public class BuyerRegisterRequest
    {
        public string BuyerName { get; set; } = default!;
        public string? BusinessId { get; set; }
        public string BusinessName { get; set; } = default!;

        public string? Email { get; set; }
        public string? Location { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public List<int> InterestedCropIds { get; set; } = new();
    }

    public class BuyerResponseDto
    {
        public Guid BuyerId { get; set; }
        public string BuyerName { get; set; } = default!;
        public string? BusinessId { get; set; }
        public string BusinessName { get; set; } = default!;

        public string? Email { get; set;}
        public string? Location { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public List<int> InterestedCropIds { get; set; } = new();
    }

    public class BuyerStatusDto
    {
        public bool IsBuyer { get; set; }
    }
}
