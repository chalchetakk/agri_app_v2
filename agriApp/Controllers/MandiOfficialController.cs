using agriApp.Services.MandiOfficials;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("mandi-official")]
    [Authorize]
    public class MandiOfficialController : ControllerBase
    {
        private readonly IMandiOfficialService _service;

        public MandiOfficialController(IMandiOfficialService service)
        {
            _service = service;
        }

        // -------------------------------------------------------
        // REGISTER
        // -------------------------------------------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] MandiOfficialRegisterRequest request)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            try
            {
                await _service.RegisterOfficialAsync(userId, request);
                return Ok(new { message = "Mandi Official registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // -------------------------------------------------------
        // STATUS
        // -------------------------------------------------------
        [HttpGet("status")]
        public async Task<IActionResult> Status()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            var exists = await _service.OfficialExistsAsync(userId);

            return Ok(new { isMandiOfficial = exists });
        }

        // -------------------------------------------------------
        // PROFILE
        // -------------------------------------------------------
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            var profile = await _service.GetOfficialProfileAsync(userId);

            if (profile == null)
                return NotFound(new { message = "Mandi official profile not found." });

            return Ok(profile);
        }
        [HttpGet("roles")]
[Authorize]
public async Task<IActionResult> GetRoles()
{
    var roles = await _service.GetRolesAsync();
    return Ok(roles);
}

    }
    public class MandiOfficialRegisterRequest
{
    public string OfficialName { get; set; } = default!;
    public string EmployeeId { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int MandiId { get; set; }
    public Guid OfficialRoleId { get; set; }
}

public class MandiOfficialProfileDto
{
    public Guid OfficialId { get; set; }
    public string OfficialName { get; set; } = default!;
    public string EmployeeId { get; set; } = default!;
    public string Email { get; set; } = default!;

    public int MandiId { get; set; }
    public string? MandiName { get; set; }
    public string? MandiLocation { get; set; }

    public Guid OfficialRoleId { get; set; }
    public string? OfficialRoleName { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class OfficialRoleDto
{
    public Guid OfficialRoleId { get; set; }
    public string OfficialRoleName { get; set; } = default!;
    public string RoleCode { get; set; } = default!;
}


}
