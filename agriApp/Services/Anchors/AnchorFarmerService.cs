using agriApp.Data;
using agriApp.DTOs.Anchors;
using agriApp.Entities.Stakeholders;
using agriApp.Services.BulkImport;
using agriApp.Services.Crops;
using agriApp.Entities.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using agriApp.Services.Templates;

namespace agriApp.Services.Anchors
{
    public class AnchorFarmerService : IAnchorFarmerService
    {
        private readonly AgriDbContext _db;
        private readonly IBulkFileParserService _parser;
        private readonly IBulkFarmerValidationService _validator;
        private readonly ICropLookupService _cropLookup;

        public AnchorFarmerService(
            AgriDbContext db,
            IBulkFileParserService parser,
            IBulkFarmerValidationService validator,
            ICropLookupService cropLookup)
        {
            _db = db;
            _parser = parser;
            _validator = validator;
            _cropLookup = cropLookup;
        }

        // ----------------------------------------------------
        // 1️⃣ REGISTER SINGLE FARMER
        // ----------------------------------------------------
        public async Task<AnchorSingleFarmerResponseDto> RegisterSingleFarmerAsync(
    Guid anchorId,
    AnchorRegisterSingleFarmerDto dto)
{
    // 1️⃣ Check if mobile already exists
    var existingUser = await _db.UserProfiles
        .FirstOrDefaultAsync(u => u.MobileNumber == dto.MobileNumber);

    if (existingUser != null)
        throw new Exception($"Mobile {dto.MobileNumber} already registered.");

    // 2️⃣ Create new UserProfile
    var user = new UserProfile(dto.MobileNumber);
    _db.UserProfiles.Add(user);
    await _db.SaveChangesAsync();

    // 3️⃣ Create farmer linked to this new user
    var farmer = new Farmer(
        userId: user.UserProfileId,
        farmerName: dto.FarmerName,
        location: dto.Location,
        profilePhotoUrl: dto.ProfilePhotoUrl
    );

    _db.Farmers.Add(farmer);
    await _db.SaveChangesAsync();

    // 4️⃣ Add Interested Crops
    foreach (var cropName in dto.InterestedCrops.Distinct())
    {
        var cropId = await _cropLookup.GetCropIdByNameAsync(cropName);
        if (cropId == null)
            throw new Exception($"Crop '{cropName}' not found in crops table.");

        _db.FarmerInterestedCrops.Add(
            new FarmerInterestedCrop(farmer.FarmerId, cropId.Value)
        );
    }

    // 5️⃣ Add Farms
    foreach (var farm in dto.Farms)
    {
        var farmRow = new FarmDetails(
            farmer.FarmerId,
            farm.FarmLocation,
            farm.PrimaryCrop,
            farm.FarmSize
        );

        _db.FarmDetails.Add(farmRow);
    }

    // 6️⃣ Create mapping between Anchor ↔ Farmer
    _db.AnchorFarmers.Add(new AnchorFarmer(anchorId, farmer.FarmerId));

    await _db.SaveChangesAsync();

    return new AnchorSingleFarmerResponseDto
    {
        FarmerId = farmer.FarmerId,
        AnchorId = anchorId,
        Message = "Farmer registered successfully."
    };
}


        // ----------------------------------------------------
        // 2️⃣ BULK REGISTRATION
        // ----------------------------------------------------
        public async Task<BulkRegisterResultDto> RegisterBulkFarmersAsync(
    Guid anchorId,
    IFormFile file)
{
    var rows = await _parser.ParseAsync(file);
    var grouped = BulkFarmerGroupedDto.GroupByMobile(rows);

    await _validator.ValidateGroupedRowsAsync(grouped);

    var result = new BulkRegisterResultDto
    {
        TotalRows = rows.Count
    };

    foreach (var group in grouped)
    {
        int firstRowNum = group.GroupRows.Min(r => r.RowNumber);

        // 1️⃣ Check if mobile already exists
        var existingUser = await _db.UserProfiles
            .FirstOrDefaultAsync(u => u.MobileNumber == group.Mobile);

        if (existingUser != null)
        {
            result.Errors.Add(new BulkRowErrorDto
            {
                RowNumber = firstRowNum,
                Column = "Mobile",
                ErrorMessage = $"Mobile {group.Mobile} already registered."
            });
            continue;
        }

        // 2️⃣ Create UserProfile
        var user = new UserProfile(group.Mobile);
        _db.UserProfiles.Add(user);
        await _db.SaveChangesAsync();

        // 3️⃣ Create Farmer
        var farmer = new Farmer(
            userId: user.UserProfileId,
            farmerName: group.FarmerName,
            location: group.Location,
            profilePhotoUrl: null
        );

        _db.Farmers.Add(farmer);
        await _db.SaveChangesAsync();

        result.TotalFarmersCreated++;

        // 4️⃣ Interested Crops
        foreach (var cropName in group.AllInterestedCrops)
        {
            var cropId = await _cropLookup.GetCropIdByNameAsync(cropName);
            if (cropId == null)
            {
                result.Errors.Add(new BulkRowErrorDto
                {
                    RowNumber = firstRowNum,
                    Column = "InterestedCrops",
                    ErrorMessage = $"Unknown crop: {cropName}"
                });
                continue;
            }

            _db.FarmerInterestedCrops.Add(
                new FarmerInterestedCrop(farmer.FarmerId, cropId.Value)
            );
        }

        // 5️⃣ Farms
        foreach (var farm in group.Farms)
        {
            _db.FarmDetails.Add(new FarmDetails(
                farmer.FarmerId,
                farm.FarmLocation,
                farm.PrimaryCrop,
                farm.FarmSize
            ));
            result.TotalFarmsCreated++;
        }

        // 6️⃣ Anchor Mapping
        _db.AnchorFarmers.Add(new AnchorFarmer(anchorId, farmer.FarmerId));

        await _db.SaveChangesAsync();
    }

    return result;
}



        // ----------------------------------------------------
        // 3️⃣ GET ALL FARMERS
        // ----------------------------------------------------
        public async Task<List<AnchorFarmerListDto>> GetFarmersAsync(Guid anchorId)
        {
            var farmers = await _db.AnchorFarmers
                .Where(a => a.AnchorId == anchorId)
                .Include(a => a.Farmer)
                 .ThenInclude(f => f!.User)     // ✅ ADD THIS
    .Include(a => a.Farmer!.FarmDetails) // ✅ ensure farms are loaded
                .Select(a => a.Farmer!)
                .ToListAsync();

            var result = new List<AnchorFarmerListDto>();

            foreach (var f in farmers)
            {
                var cropIds = await _db.FarmerInterestedCrops
                    .Where(x => x.FarmerId == f.FarmerId)
                    .Select(x => x.CropId)
                    .ToListAsync();

                var cropNames = await _cropLookup.GetNamesByIds(cropIds);

                result.Add(new AnchorFarmerListDto
                {
                    FarmerId = f.FarmerId,
                    FarmerName = f.FarmerName,
                    MobileNumber = f.User?.MobileNumber ?? "",   // ✅ ADD THIS
                    Location = f.Location,
                    TotalFarms = f.FarmDetails.Count,
                    InterestedCrops = cropNames
                });
            }

            return result;
        }

        // ----------------------------------------------------
        // 4️⃣ GET FARMER DETAILS
        // ----------------------------------------------------
        public async Task<AnchorFarmerDetailDto> GetFarmerDetailAsync(Guid anchorId, Guid farmerId)
        {
            var belongs = await _db.AnchorFarmers
                .AnyAsync(x => x.AnchorId == anchorId && x.FarmerId == farmerId);

            if (!belongs)
                throw new Exception("This farmer does not belong to your anchor.");

            var farmer = await _db.Farmers
             .Include(f => f.User)                // ✅ ADD THIS
                .Include(f => f.FarmDetails)
                .Include(f => f.InterestedCrops)
                .FirstAsync(f => f.FarmerId == farmerId);

            var cropNames = await _cropLookup.GetNamesByIds(
                farmer.InterestedCrops.Select(x => x.CropId).ToList()
            );

            return new AnchorFarmerDetailDto
            {
                FarmerId = farmer.FarmerId,
                FarmerName = farmer.FarmerName,
                MobileNumber = farmer.User?.MobileNumber ?? "", // ✅ ADD THIS
                Location = farmer.Location,
                ProfilePhotoUrl = farmer.ProfilePhotoUrl,
                InterestedCrops = cropNames,
                Farms = farmer.FarmDetails.Select(fd => new FarmDetailDto
                {
                    FarmId = fd.FarmId,
                    FarmLocation = fd.FarmLocation,
                    PrimaryCrop = fd.PrimaryCrop,
                    FarmSize = fd.FarmSize
                }).ToList(),
                CreatedAt = farmer.CreatedAt,
                UpdatedAt = farmer.UpdatedAt
            };
        }

        // ----------------------------------------------------
        // 5️⃣ Templates (MemoryStream required)
        // ----------------------------------------------------
        public Task<MemoryStream> GenerateCsvTemplateAsync()
        {
            return Task.FromResult(TemplateFactory.CreateCsvTemplate());
        }

        public Task<MemoryStream> GenerateExcelTemplateAsync()
        {
            return Task.FromResult(TemplateFactory.CreateExcelTemplate());
        }
    }
}
