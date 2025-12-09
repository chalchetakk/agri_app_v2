using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using agriApp.Services.Anchors;
using agriApp.DTOs.Anchors;
using System.IdentityModel.Tokens.Jwt;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("anchor")]
    [Authorize]   // All endpoints require a logged-in user
    public class AnchorController : ControllerBase
    {
        private readonly IAnchorService _anchorService;

        public AnchorController(IAnchorService anchorService)
        {
            _anchorService = anchorService;
        }

        // ------------------------------------------------------------
        // HELPER: Get UserId from Access Token
        // ------------------------------------------------------------
        private Guid GetUserId()
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                throw new Exception("UserId not found in token.");

            return Guid.Parse(userId);
        }

        // ------------------------------------------------------------
        // 1️⃣ REGISTER ANCHOR
        // ------------------------------------------------------------
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAnchor([FromBody] RegisterAnchorRequest request)
        {
            try
            {
                var userId = GetUserId();

                await _anchorService.RegisterAnchorAsync(userId, request);

                return Ok(new { message = "Anchor registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ------------------------------------------------------------
        // 2️⃣ GET ANCHOR PROFILE
        // ------------------------------------------------------------
        [HttpGet("profile")]
        public async Task<IActionResult> GetAnchorProfile()
        {
            try
            {
                var userId = GetUserId();

                var anchor = await _anchorService.GetAnchorProfileAsync(userId);
                if (anchor == null)
                    return NotFound(new { message = "Anchor profile not found." });

                return Ok(new
                {
                    anchorId = anchor.AnchorId,
                    userId = anchor.UserId,
                    companyName = anchor.CompanyName,
                    registrationNumber = anchor.RegistrationNumber,
                    companyAddress = anchor.CompanyAddress,
                    contactPersonName = anchor.ContactPersonName,
                    email = anchor.Email,
                    contactPersonNum = anchor.ContactPersonNum,
                    gstNumber = anchor.GSTNumber,
                    estimatedFarmersNum = anchor.EstimatedFarmersNum,
                    businessDescription = anchor.BusinessDescription,
                    createdAt = anchor.CreatedAt,
                    updatedAt = anchor.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ------------------------------------------------------------
        // 3️⃣ CHECK ANCHOR STATUS
        // ------------------------------------------------------------
        [HttpGet("status")]
        public async Task<IActionResult> GetAnchorStatus()
        {
            try
            {
                var userId = GetUserId();
                var isAnchor = await _anchorService.IsUserAnchorAsync(userId);

                return Ok(new { isAnchor });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ------------------------------------------------------------
        // 4️⃣ UPDATE ANCHOR PROFILE
        // ------------------------------------------------------------
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAnchor([FromBody] UpdateAnchorRequest request)
        {
            try
            {
                var userId = GetUserId();

                await _anchorService.UpdateAnchorAsync(userId, request);

                return Ok(new { message = "Anchor profile updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
