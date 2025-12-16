using Microsoft.EntityFrameworkCore;
using agriApp.Data;
using agriApp.Dtos.Buyers;

namespace agriApp.Services.Buyers
{
    public class BuyerSearchService : IBuyerSearchService
    {
        private readonly AgriDbContext _db;

        public BuyerSearchService(AgriDbContext db)
        {
            _db = db;
        }

        public async Task<List<BuyerSearchResultDto>> SearchBuyersAsync(string query)
        {
            query = query.Trim();

            var results = new List<BuyerSearchResultDto>();

            if (string.IsNullOrWhiteSpace(query))
                return results;

            // ===============================
            // 1️⃣ Search by MOBILE (UserProfiles)
            // ===============================
            var usersByMobile = await _db.UserProfiles
                .Where(u => u.MobileNumber.Contains(query))
                .ToListAsync();

            var userIds = usersByMobile
                .Select(u => u.UserProfileId)
                .ToList();

            var buyersForUsers = await _db.Buyers
                .Where(b => userIds.Contains(b.UserId))
                .ToListAsync();

            foreach (var user in usersByMobile)
            {
                var buyer = buyersForUsers.FirstOrDefault(b => b.UserId == user.UserProfileId);

                if (buyer != null)
                {
                    results.Add(new BuyerSearchResultDto
                    {
                        ResultType = "BUYER",
                        BuyerId = buyer.BuyerId,
                        UserId = user.UserProfileId,
                        Name = buyer.BuyerName,
                        MobileNumber = user.MobileNumber
                    });
                }
                else
                {
                    results.Add(new BuyerSearchResultDto
                    {
                        ResultType = "NOT_BUYER",
                        BuyerId = null,
                        UserId = user.UserProfileId,
                        Name = null,
                        MobileNumber = user.MobileNumber,
                        Message = "User is registered but not as buyer"
                    });
                }
            }

            // ===============================
            // 2️⃣ Search by BUYER NAME
            // ===============================
            var buyersByName = await _db.Buyers
                .Include(b => b.User)
                .Where(b => b.BuyerName.ToLower().Contains(query.ToLower()))
                .ToListAsync();

            foreach (var buyer in buyersByName)
            {
                // Skip if already added from mobile search
                if (results.Any(r => r.UserId == buyer.UserId))
                    continue;

                results.Add(new BuyerSearchResultDto
                {
                    ResultType = "BUYER",
                    BuyerId = buyer.BuyerId,
                    UserId = buyer.UserId,
                    Name = buyer.BuyerName,
                    MobileNumber = buyer.User!.MobileNumber
                });
            }

            return results;
        }
    }
}
