using System;
using System.Linq;
using System.Threading.Tasks;
using agriApp.Data;
// using agriApp.DTOs.Buyers;
using agriApp.Entities.Stakeholders;
using Microsoft.EntityFrameworkCore;
using agriApp.Controllers;

namespace agriApp.Services.Buyers
{
    public class BuyerService : IBuyerService
    {
        private readonly AgriDbContext _db;

        public BuyerService(AgriDbContext db)
        {
            _db = db;
        }

        // -------------------------------------------------------
        // CHECK BUYER STATUS
        // -------------------------------------------------------
        public async Task<bool> IsBuyerAsync(Guid userId)
        {
            return await _db.Buyers.AnyAsync(b => b.UserId == userId);
        }

        // -------------------------------------------------------
        // REGISTER BUYER
        // -------------------------------------------------------
        public async Task<BuyerResponseDto> RegisterBuyerAsync(Guid userId, BuyerRegisterRequest request)
        {
            var exists = await _db.Buyers.AnyAsync(b => b.UserId == userId);
            if (exists)
                throw new Exception("Buyer already registered.");

            var buyer = new Buyer(
                userId,
                request.BuyerName,
                request.BusinessName,
                request.BusinessId,
                request.Location,
                request.ProfilePhotoUrl
            );

            _db.Buyers.Add(buyer);

            // Save crops
            foreach (var cropId in request.InterestedCropIds)
            {
                var entry = new BuyerInterestedCrop(buyer.BuyerId, cropId);
                _db.BuyerInterestedCrops.Add(entry);
            }

            await _db.SaveChangesAsync();

            return new BuyerResponseDto
            {
                BuyerId = buyer.BuyerId,
                BuyerName = buyer.BuyerName,
                BusinessId = buyer.BusinessId,
                BusinessName = buyer.BusinessName,
                Location = buyer.Location,
                ProfilePhotoUrl = buyer.ProfilePhotoUrl,
                InterestedCropIds = request.InterestedCropIds
            };
        }

        // -------------------------------------------------------
        // GET BUYER PROFILE
        // -------------------------------------------------------
        public async Task<BuyerResponseDto?> GetBuyerProfileAsync(Guid userId)
        {
            var buyer = await _db.Buyers
                .Include(b => b.InterestedCrops)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (buyer == null) return null;

            return new BuyerResponseDto
            {
                BuyerId = buyer.BuyerId,
                BuyerName = buyer.BuyerName,
                BusinessId = buyer.BusinessId,
                BusinessName = buyer.BusinessName,
                Location = buyer.Location,
                ProfilePhotoUrl = buyer.ProfilePhotoUrl,
                InterestedCropIds = buyer.InterestedCrops.Select(c => c.CropId).ToList()
            };
        }
    }
}
