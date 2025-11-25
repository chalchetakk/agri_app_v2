using agriApp.Services.Farmers;
using agriApp.Data;   // 👈 add this
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;   // 👈 REQUIRED for Include, AnyAsync


namespace agriApp.Controllers
{
    [ApiController]
    [Route("farmer")]
    [Authorize]
    public class FarmerController : ControllerBase
    {
        private readonly IFarmerService _farmerService;
        private readonly AgriDbContext _db;     // 👈 add this

        public FarmerController(IFarmerService farmerService, AgriDbContext db)
        {
            _farmerService = farmerService;
            _db = db;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterFarmer([FromBody] FarmerRegisterRequest request)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            try
            {
                await _farmerService.RegisterFarmerAsync(userId, request);
                return Ok(new { message = "Farmer registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("status")]
[Authorize]
public async Task<IActionResult> FarmerStatus()
{
    var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
    if (userIdClaim == null)
        return Unauthorized();

    var userId = Guid.Parse(userIdClaim);

    var exists = await _db.Farmers.AnyAsync(f => f.UserId == userId);

    return Ok(new { isFarmer = exists });
}
[HttpGet("profile")]
[Authorize]
public async Task<IActionResult> GetFarmerProfile()
{
    var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
    if (userIdClaim == null)
        return Unauthorized();

    var userId = Guid.Parse(userIdClaim);

    var farmer = await _db.Farmers
        .Include(f => f.InterestedCrops)
        .Include(f => f.FarmDetails)
        .FirstOrDefaultAsync(f => f.UserId == userId);

    if (farmer == null)
        return NotFound(new { message = "Farmer profile not found." });

    return Ok(new
    {
        farmerId = farmer.FarmerId,
        farmerName = farmer.FarmerName,
        profilePhotoUrl = farmer.ProfilePhotoUrl,
        location = farmer.Location,
        interestedCropIds = farmer.InterestedCrops.Select(x => x.CropId),
        farmDetails = farmer.FarmDetails.Select(fd => new {
            farmId = fd.FarmId,
            farmLocation = fd.FarmLocation,
            primaryCrop = fd.PrimaryCrop,
            farmSize = fd.FarmSize
        }),
        createdAt = farmer.CreatedAt,
        updatedAt = farmer.UpdatedAt
    });
}

    }

    

    // Dtos
    public class FarmerRegisterRequest
{
    public string FarmerName { get; set; } = default!;
    public string? ProfilePhotoUrl { get; set; }
    public string? Location { get; set; }
    public List<int> CropIds { get; set; } = new();
    public List<FarmDetailItem> FarmDetails { get; set; } = new();
}

public class FarmDetailItem
{
    public string FarmLocation { get; set; } = default!;
    public string PrimaryCrop { get; set; } = default!;
    public float FarmSize { get; set; }
}

public class FarmerStatusDto
{
    public bool Exists { get; set; }
}

public class FarmerProfileDto
{
    public Guid FarmerId { get; set; }
    public string FarmerName { get; set; } = default!;
    public string? ProfilePhotoUrl { get; set; }
    public string? Location { get; set; }
    public List<int> InterestedCropIds { get; set; } = new();
    public List<FarmDetailDto> FarmDetails { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class FarmDetailDto
{
    public Guid FarmId { get; set; }
    public string FarmLocation { get; set; } = default!;
    public string PrimaryCrop { get; set; } = default!;
    public float FarmSize { get; set; }
}


}
