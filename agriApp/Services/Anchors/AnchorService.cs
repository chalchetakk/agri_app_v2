using System;
using System.Threading.Tasks;
using agriApp.Data;
using agriApp.Entities.Stakeholders;
using Microsoft.EntityFrameworkCore;
using agriApp.DTOs.Anchors;

namespace agriApp.Services.Anchors
{
    public class AnchorService : IAnchorService
    {
        private readonly AgriDbContext _db;

        public AnchorService(AgriDbContext db)
        {
            _db = db;
        }

        // -----------------------------------------------------
        // REGISTER ANCHOR
        // -----------------------------------------------------
        public async Task RegisterAnchorAsync(Guid userId, RegisterAnchorRequest request)
        {
            // Check if this user already registered as anchor
            var exists = await _db.Anchors.AnyAsync(a => a.UserId == userId);
            if (exists)
                throw new Exception("Anchor profile already exists for this user.");

            if (request.EstimatedFarmersNum <= 0)
                throw new Exception("Estimated farmers must be greater than 0.");

            // Create entity
            var anchor = new Anchor(
                userId,
                request.CompanyName,
                request.RegistrationNumber,
                request.CompanyAddress,
                request.ContactPersonName,
                request.Email,
                request.ContactPersonNum,
                request.EstimatedFarmersNum,
                request.GSTNumber,
                request.BusinessDescription
            );

            _db.Anchors.Add(anchor);
            await _db.SaveChangesAsync();
        }

        // -----------------------------------------------------
        // GET ANCHOR PROFILE
        // -----------------------------------------------------
        public async Task<Anchor?> GetAnchorProfileAsync(Guid userId)
        {
            return await _db.Anchors
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }

        // -----------------------------------------------------
        // CHECK IF USER IS ANCHOR
        // -----------------------------------------------------
        public async Task<bool> IsUserAnchorAsync(Guid userId)
        {
            return await _db.Anchors.AnyAsync(a => a.UserId == userId);
        }

        // -----------------------------------------------------
        // UPDATE ANCHOR PROFILE
        // -----------------------------------------------------
        public async Task UpdateAnchorAsync(Guid userId, UpdateAnchorRequest request)
        {
            var anchor = await _db.Anchors.FirstOrDefaultAsync(a => a.UserId == userId);
            if (anchor == null)
                throw new Exception("Anchor profile not found for this user.");

            // Update only provided fields
            if (!string.IsNullOrWhiteSpace(request.CompanyName))
                anchor.CompanyName = request.CompanyName;

            if (!string.IsNullOrWhiteSpace(request.RegistrationNumber))
                anchor.RegistrationNumber = request.RegistrationNumber;

            if (!string.IsNullOrWhiteSpace(request.CompanyAddress))
                anchor.CompanyAddress = request.CompanyAddress;

            if (!string.IsNullOrWhiteSpace(request.ContactPersonName))
                anchor.ContactPersonName = request.ContactPersonName;

            if (!string.IsNullOrWhiteSpace(request.Email))
                anchor.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.ContactPersonNum))
                anchor.ContactPersonNum = request.ContactPersonNum;

            if (!string.IsNullOrWhiteSpace(request.GSTNumber))
                anchor.GSTNumber = request.GSTNumber;

            if (request.EstimatedFarmersNum != null)
            {
                if (request.EstimatedFarmersNum <= 0)
                    throw new Exception("Estimated farmers must be greater than 0.");

                anchor.EstimatedFarmersNum = request.EstimatedFarmersNum.Value;
            }

            if (!string.IsNullOrWhiteSpace(request.BusinessDescription))
                anchor.BusinessDescription = request.BusinessDescription;

            // Update timestamp
            anchor.Touch();

            _db.Anchors.Update(anchor);
            await _db.SaveChangesAsync();
        }
    }
}
