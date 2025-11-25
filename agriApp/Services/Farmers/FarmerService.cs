using agriApp.Data;
using agriApp.Entities.Stakeholders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using agriApp.Controllers;

namespace agriApp.Services.Farmers
{
    public class FarmerService : IFarmerService
    {
        private readonly AgriDbContext _db;

        public FarmerService(AgriDbContext db)
        {
            _db = db;
        }

        public async Task RegisterFarmerAsync(Guid userId, FarmerRegisterRequest request)
        {
            // Validate required
            if (request.FarmDetails == null || request.FarmDetails.Count == 0)
                throw new Exception("At least one farm entry is required.");

            // Prevent double registration
            var alreadyExists = await _db.Farmers.AnyAsync(f => f.UserId == userId);
            if (alreadyExists)
                throw new Exception("Farmer profile already exists.");

            // Begin transaction
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                // Create farmer row
                var farmer = new Farmer(
                    userId,
                    request.FarmerName,
                    request.Location,
                    request.ProfilePhotoUrl
                );

                _db.Farmers.Add(farmer);
                await _db.SaveChangesAsync();

                // Add interested crop rows
                foreach (var cropId in request.CropIds)
                {
                    var interest = new FarmerInterestedCrop(farmer.FarmerId, cropId);
                    _db.FarmerInterestedCrops.Add(interest);
                }

                // Add farm details rows
                foreach (var farm in request.FarmDetails)
                {
                    var fd = new FarmDetails(
                        farmer.FarmerId,
                        farm.FarmLocation,
                        farm.PrimaryCrop,
                        farm.FarmSize
                    );

                    _db.FarmDetails.Add(fd);
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
