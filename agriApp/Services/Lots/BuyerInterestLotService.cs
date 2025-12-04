using Microsoft.EntityFrameworkCore;
using agriApp.Data;
using agriApp.Controllers;
using agriApp.Entities.Lots;
using agriApp.Entities.Stakeholders;

namespace agriApp.Services.Lots
{
    public class BuyerInterestLotService : IBuyerInterestLotService
    {
        private readonly AgriDbContext _db;

        public BuyerInterestLotService(AgriDbContext db)
        {
            _db = db;
        }

        // ───────────────────────────────────────────────────────────────
        // 1️⃣ GET ALL LOTS AVAILABLE FOR BUYER (preRegistered only)
        // ───────────────────────────────────────────────────────────────
        public async Task<List<BuyerLotsController.BuyerLotListItemDto>> GetLotsAvailableForBuyerAsync(Guid buyerUserId)
        {
            var lots = await _db.PreRegisteredLots
                .Include(l => l.Crop)
                .Include(l => l.Mandi)
                .Where(l => l.Status == "preRegistered")
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            return lots.Select(l => new BuyerLotsController.BuyerLotListItemDto
            {
                PreLotId = l.PreLotId,
                CropName = l.Crop?.CropName ?? "",
                Quantity = l.Quantity,
                Grade = l.Grade,
                LotImageUrl = l.LotImageUrl,
                Status = l.Status,
                MandiId = l.MandiId,
                MandiName = l.Mandi?.MandiName ?? ""
            }).ToList();
        }

        // ───────────────────────────────────────────────────────────────
        // 2️⃣ GET LOT DETAIL + buyer’s bid history
        // ───────────────────────────────────────────────────────────────
        public async Task<BuyerLotsController.BuyerLotDetailDto?> GetLotDetailForBuyerAsync(string preLotId, Guid buyerUserId)
        {
            // Load lot
            var lot = await _db.PreRegisteredLots
                .Include(l => l.Crop)
                .Include(l => l.Mandi)
                .FirstOrDefaultAsync(l => l.PreLotId == preLotId && l.Status == "preRegistered");

            if (lot == null)
                return null;

            // Get buyer profile
            var buyer = await _db.Buyers.FirstOrDefaultAsync(b => b.UserId == buyerUserId);
            if (buyer == null)
                return null;

            // Check if buyer has placed a bid
            var myBid = await _db.BuyerInterestLots
                .FirstOrDefaultAsync(b => b.PreLotId == preLotId && b.BuyerId == buyer.BuyerId);

            return new BuyerLotsController.BuyerLotDetailDto
            {
                PreLotId = lot.PreLotId,
                CropName = lot.Crop?.CropName ?? "",
                Quantity = lot.Quantity,
                Grade = lot.Grade,
                LotImageUrl = lot.LotImageUrl,
                QrCodeUrl = lot.QrCodeUrl,
                Status = lot.Status,
                MandiId = lot.MandiId,
                MandiName = lot.Mandi?.MandiName ?? "",
                MyBidAmount = myBid?.BuyerBidAmount,
                MyBidStatus = myBid?.Status
            };
        }

        // ───────────────────────────────────────────────────────────────
        // 3️⃣ BUYER PLACES A BID
        // ───────────────────────────────────────────────────────────────
        public async Task<BuyerLotsController.BuyerPlacedBidDto> PlaceBidAsync(Guid buyerUserId, string preLotId, decimal bidAmount)
        {
            var buyer = await _db.Buyers.FirstOrDefaultAsync(b => b.UserId == buyerUserId);
            if (buyer == null)
                throw new Exception("Buyer not registered.");

            // Check if lot exists
            var lotExists = await _db.PreRegisteredLots.AnyAsync(p => p.PreLotId == preLotId);
            if (!lotExists)
                throw new Exception("Lot does not exist.");

            // Check if buyer already placed a bid → update it
            var existingBid = await _db.BuyerInterestLots
                .FirstOrDefaultAsync(b => b.PreLotId == preLotId && b.BuyerId == buyer.BuyerId);

            if (existingBid != null)
            {
                existingBid.BuyerBidAmount = bidAmount;
                existingBid.Status = "pending";
                existingBid.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                return ToDto(existingBid, preLotId);
            }

            // Create new bid
            var newBid = new BuyerInterestLot
            {
                PreLotId = preLotId,
                BuyerId = buyer.BuyerId,
                BuyerBidAmount = bidAmount,
                Status = "pending",
                CreatedAt = DateTime.UtcNow
            };

            _db.BuyerInterestLots.Add(newBid);
            await _db.SaveChangesAsync();

            return ToDto(newBid, preLotId);
        }

        private BuyerLotsController.BuyerPlacedBidDto ToDto(BuyerInterestLot b, string preLotId)
        {
            return new BuyerLotsController.BuyerPlacedBidDto
            {
                BuyerInterestLotId = b.BuyerInterestLotId,
                PreLotId = preLotId,
                BidAmount = (decimal)b.BuyerBidAmount,
                Status = b.Status,
                CreatedAt = b.CreatedAt
            };
        }

        // ───────────────────────────────────────────────────────────────
        // 4️⃣ GET ALL BIDS PLACED BY BUYER
        // ───────────────────────────────────────────────────────────────
        public async Task<List<BuyerLotsController.BuyerPlacedBidDto>> GetMyPlacedBidsAsync(Guid buyerUserId)
        {
            var buyer = await _db.Buyers.FirstOrDefaultAsync(b => b.UserId == buyerUserId);
            if (buyer == null)
                return new List<BuyerLotsController.BuyerPlacedBidDto>();

            var bids = await _db.BuyerInterestLots
                .Where(b => b.BuyerId == buyer.BuyerId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return bids.Select(b => new BuyerLotsController.BuyerPlacedBidDto
            {
                BuyerInterestLotId = b.BuyerInterestLotId,
                PreLotId = b.PreLotId,
                BidAmount = (decimal)b.BuyerBidAmount,
                Status = b.Status,
                CreatedAt = b.CreatedAt
            }).ToList();
        }
    }
}
