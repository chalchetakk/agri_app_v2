using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using agriApp.Data;
using agriApp.Entities.Lots;
using agriApp.Entities.Stakeholders;
using agriApp.Dtos.Lots;
using agriApp.Services.Lots;

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
public async Task<LiveAuctionLotDto?> GetByIdAsync(int liveLotId)
{
    var lot = await _db.LiveAuctionLots
        .Include(l => l.ArrivedLot)
            .ThenInclude(a => a.Crop)
        .FirstOrDefaultAsync(l => l.LiveAuctionLotId == liveLotId);

    if (lot == null)
        return null;

    return lot.ToDto();
}

        public async Task<LiveAuctionLotDto?> MarkSoldAsync(
    int liveAuctionLotId,
    float finalPrice,
    Guid? buyerId,
    string? buyerName,
    string? buyerMobile)
{
    using var tx = await _db.Database.BeginTransactionAsync();

    // Load lot + preregistered data
    var lot = await _db.LiveAuctionLots
        .Include(x => x.ArrivedLot)
            .ThenInclude(x => x.PreRegisteredLot)
        .FirstOrDefaultAsync(x => x.LiveAuctionLotId == liveAuctionLotId);

    if (lot == null)
        throw new Exception("Live auction lot not found.");

    // ⭐ NEW — Ensure parent auction has started
    var parentAuction = await _db.Auctions
        .FirstOrDefaultAsync(a => a.AuctionId == lot.AuctionId);

    if (parentAuction == null)
        throw new Exception("Auction not found for this lot.");

    if (parentAuction.Status != "started")
        throw new Exception("Cannot update lot status — auction has not started.");

    // Existing: lot must be pending
    if (lot.AuctionStatus != "pending")
        throw new Exception("Auction lot not pending.");

    // Buyer validation
    if (buyerId == null && string.IsNullOrWhiteSpace(buyerName))
        throw new Exception("Buyer name is required for non-app buyer.");

    if (buyerId != null)
    {
        var buyer = await _db.Buyers
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.BuyerId == buyerId)
            ?? throw new Exception("Invalid buyerId.");

        lot.BuyerId = buyerId;
        lot.BuyerName = !string.IsNullOrWhiteSpace(buyer.BuyerName)
            ? buyer.BuyerName
            : buyer.User?.MobileNumber;
        lot.BuyerMobile = buyer.User?.MobileNumber;
    }
    else
    {
        lot.BuyerId = null;
        lot.BuyerName = buyerName;
        lot.BuyerMobile = buyerMobile;
    }

    // Update lot 
    lot.FinalPrice = finalPrice;
    lot.AuctionStatus = "sold";
    lot.UpdatedAt = DateTime.UtcNow;

    _db.LiveAuctionLots.Update(lot);

    // Update ArrivedLot
    var arrived = lot.ArrivedLot!;
    arrived.Status = "sold";
    arrived.UpdatedAt = DateTime.UtcNow;

    _db.ArrivedLots.Update(arrived);

    // Update PreRegisteredLot
    if (arrived.PreRegisteredLot != null)
    {
        arrived.PreRegisteredLot.Status = "sold";
        arrived.PreRegisteredLot.SellingAmount = finalPrice;
        arrived.PreRegisteredLot.UpdatedAt = DateTime.UtcNow;

        _db.PreRegisteredLots.Update(arrived.PreRegisteredLot);
    }

    await _db.SaveChangesAsync();
    await tx.CommitAsync();

    return lot.ToDto();
}
public async Task<LiveAuctionLotDto?> MarkUnsoldAsync(int liveAuctionLotId)
{
    using var tx = await _db.Database.BeginTransactionAsync();

    // Load lot + preregistered data
    var lot = await _db.LiveAuctionLots
        .Include(x => x.ArrivedLot)
            .ThenInclude(x => x.PreRegisteredLot)
        .FirstOrDefaultAsync(x => x.LiveAuctionLotId == liveAuctionLotId);

    if (lot == null)
        throw new Exception("Live auction lot not found.");

    // ⭐ NEW — Ensure parent auction has started
    var parentAuction = await _db.Auctions
        .FirstOrDefaultAsync(a => a.AuctionId == lot.AuctionId);

    if (parentAuction == null)
        throw new Exception("Auction not found for this lot.");

    if (parentAuction.Status != "started")
        throw new Exception("Cannot update lot status — auction has not started.");

    // Ensure pending
    if (lot.AuctionStatus != "pending")
        throw new Exception("Auction lot not pending.");

    // Update lot
    lot.AuctionStatus = "unsold";
    lot.FinalPrice = null;
    lot.BuyerId = null;
    lot.BuyerName = null;
    lot.BuyerMobile = null;
    lot.UpdatedAt = DateTime.UtcNow;

    _db.LiveAuctionLots.Update(lot);

    // Update ArrivedLot
    var arrived = lot.ArrivedLot!;
    arrived.Status = "unsold";
    arrived.UpdatedAt = DateTime.UtcNow;

    _db.ArrivedLots.Update(arrived);

    // Update PreRegisteredLot
    if (arrived.PreRegisteredLot != null)
    {
        arrived.PreRegisteredLot.Status = "unsold";
        arrived.PreRegisteredLot.SellingAmount = null;
        arrived.PreRegisteredLot.UpdatedAt = DateTime.UtcNow;

        _db.PreRegisteredLots.Update(arrived.PreRegisteredLot);
    }

    await _db.SaveChangesAsync();
    await tx.CommitAsync();

    return lot.ToDto();
}

    }
}
