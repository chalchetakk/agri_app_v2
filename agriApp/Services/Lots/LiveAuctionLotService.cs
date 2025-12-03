using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using agriApp.Data;
using agriApp.Entities.Lots;
using agriApp.Entities.Stakeholders;

namespace agriApp.Services.Lots
{
    public class LiveAuctionLotService : ILiveAuctionLotService
    {
        private readonly AgriDbContext _db;

        public LiveAuctionLotService(AgriDbContext db)
        {
            _db = db;
        }

        // Create LiveAuctionLot when ArrivedLot becomes readyForAuction
        public async Task<LiveAuctionLot> CreateAuctionEntryAsync(int arrivedLotId)
        {
            var entity = new LiveAuctionLot
            {
                ArrivedLotId = arrivedLotId,
                AuctionStatus = "pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.LiveAuctionLots.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        // Mark SOLD (atomic update of 3 tables)
        public async Task<LiveAuctionLot?> MarkSoldAsync(
            int liveAuctionLotId,
            float finalPrice,
            Guid? buyerId,
            string? buyerName,
            string? buyerMobile)
        {
            using var tx = await _db.Database.BeginTransactionAsync();

            var auction = await _db.LiveAuctionLots
                .Include(x => x.ArrivedLot)
                    .ThenInclude(x => x.PreRegisteredLot)
                .FirstOrDefaultAsync(x => x.LiveAuctionLotId == liveAuctionLotId);

            if (auction == null) return null;
            if (auction.AuctionStatus != "pending")
                throw new Exception("Auction lot not pending.");

            if (buyerId == null && string.IsNullOrWhiteSpace(buyerName))
                throw new Exception("Buyer name is required for non-app buyer.");

            // Fill offline buyer or auto-fill from BuyerId
            if (buyerId != null)
            {
                var buyer = await _db.Buyers
                    .Include(b => b.User)
                    .FirstOrDefaultAsync(x => x.BuyerId == buyerId)
                    ?? throw new Exception("Invalid buyerId.");

                auction.BuyerId = buyerId;
                // prefer explicit BuyerName if buyer.BuyerName exists, otherwise fallback
                auction.BuyerName = !string.IsNullOrWhiteSpace(buyer.BuyerName) ? buyer.BuyerName : buyer.User?.MobileNumber;
                auction.BuyerMobile = buyer.User?.MobileNumber;
            }
            else
            {
                auction.BuyerId = null;
                auction.BuyerName = buyerName;
                auction.BuyerMobile = buyerMobile;
            }

            auction.FinalPrice = finalPrice;
            auction.AuctionStatus = "sold";
            auction.UpdatedAt = DateTime.UtcNow;
            _db.LiveAuctionLots.Update(auction);

            // Update ArrivedLot
            var arrived = auction.ArrivedLot;
            if (arrived == null)
                throw new Exception("Arrived lot missing for the auction record.");

            arrived.Status = "sold";
            arrived.UpdatedAt = DateTime.UtcNow;
            _db.ArrivedLots.Update(arrived);

            // Update PreRegisteredLot if exists
            if (arrived.PreRegisteredLot != null)
            {
                arrived.PreRegisteredLot.Status = "sold";
                arrived.PreRegisteredLot.SellingAmount = finalPrice;
                arrived.PreRegisteredLot.UpdatedAt = DateTime.UtcNow;

                _db.PreRegisteredLots.Update(arrived.PreRegisteredLot);
            }

            await _db.SaveChangesAsync();
            await tx.CommitAsync();

            return auction;
        }

        // Mark UNSOLD
        public async Task<LiveAuctionLot?> MarkUnsoldAsync(int liveAuctionLotId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();

            var auction = await _db.LiveAuctionLots
                .Include(x => x.ArrivedLot)
                    .ThenInclude(x => x.PreRegisteredLot)
                .FirstOrDefaultAsync(x => x.LiveAuctionLotId == liveAuctionLotId);

            if (auction == null) return null;
            if (auction.AuctionStatus != "pending")
                throw new Exception("Auction lot not pending.");

            auction.AuctionStatus = "unsold";
            auction.FinalPrice = null;
            auction.BuyerId = null;
            auction.BuyerName = null;
            auction.BuyerMobile = null;
            auction.UpdatedAt = DateTime.UtcNow;

            _db.LiveAuctionLots.Update(auction);

            // ArrivedLot update
            var arrived = auction.ArrivedLot;
            if (arrived == null)
                throw new Exception("Arrived lot missing for the auction record.");

            arrived.Status = "unsold";
            arrived.UpdatedAt = DateTime.UtcNow;

            _db.ArrivedLots.Update(arrived);

            // PreRegisteredLot update
            if (arrived.PreRegisteredLot != null)
            {
                arrived.PreRegisteredLot.Status = "unsold";
                arrived.PreRegisteredLot.SellingAmount = null;
                arrived.PreRegisteredLot.UpdatedAt = DateTime.UtcNow;

                _db.PreRegisteredLots.Update(arrived.PreRegisteredLot);
            }

            await _db.SaveChangesAsync();
            await tx.CommitAsync();

            return auction;
        }
    }
}
