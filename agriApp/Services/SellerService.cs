using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using agriApp.Data;
using agriApp.Entities.Stakeholders;
using agriApp.Controllers;

namespace agriApp.Services.Sellers
{
    public class SellerService : ISellerService
    {
        private readonly AgriDbContext _db;

        public SellerService(AgriDbContext db)
        {
            _db = db;
        }

        public async Task RegisterSellerAsync(Guid userId, SellerRegisterRequest request)
        {
            var exists = await _db.Sellers.AnyAsync(s => s.UserId == userId);
            if (exists)
                throw new Exception("Already registered as Seller.");

            var seller = new Seller(
                userId: userId,
                sellerName: request.SellerName,
                businessName: request.BusinessName,
                location: request.Location,
                photoUrl: request.ProfilePhotoUrl
            );

            _db.Sellers.Add(seller);
            await _db.SaveChangesAsync();

            if (request.CropIds != null && request.CropIds.Count > 0)
            {
                foreach (var cropId in request.CropIds)
                {
                    _db.SellerInterestedCrops.Add(new SellerInterestedCrop
                    {
                        SellerId = seller.SellerId,
                        CropId = cropId
                    });
                }

                await _db.SaveChangesAsync();
            }
        }

        public async Task<bool> IsSellerExistsAsync(Guid userId)
        {
            return await _db.Sellers.AnyAsync(s => s.UserId == userId);
        }
    }
}
