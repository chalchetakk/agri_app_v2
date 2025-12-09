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

        // Mark SOLD (atomic update of 3 tables)
        public async Task<LiveAuctionLotDto?> MarkSoldAsync(
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

    if (buyerId != null)
    {
        var buyer = await _db.Buyers
            .Include(b => b.User)
            .FirstOrDefaultAsync(x => x.BuyerId == buyerId)
            ?? throw new Exception("Invalid buyerId.");

        auction.BuyerId = buyerId;
        auction.BuyerName = !string.IsNullOrWhiteSpace(buyer.BuyerName)
            ? buyer.BuyerName
            : buyer.User?.MobileNumber;
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

    // ArrivedLot update
    var arrived = auction.ArrivedLot!;
    arrived.Status = "sold";
    arrived.UpdatedAt = DateTime.UtcNow;
    _db.ArrivedLots.Update(arrived);

    // PreLot update
    if (arrived.PreRegisteredLot != null)
    {
        arrived.PreRegisteredLot.Status = "sold";
        arrived.PreRegisteredLot.SellingAmount = finalPrice;
        arrived.PreRegisteredLot.UpdatedAt = DateTime.UtcNow;

        _db.PreRegisteredLots.Update(arrived.PreRegisteredLot);
    }

    await _db.SaveChangesAsync();
    await tx.CommitAsync();

    return auction.ToDto();
}

        // Mark UNSOLD
        public async Task<LiveAuctionLotDto?> MarkUnsoldAsync(int liveAuctionLotId)
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

    var arrived = auction.ArrivedLot!;
    arrived.Status = "unsold";
    arrived.UpdatedAt = DateTime.UtcNow;

    _db.ArrivedLots.Update(arrived);

    if (arrived.PreRegisteredLot != null)
    {
        arrived.PreRegisteredLot.Status = "unsold";
        arrived.PreRegisteredLot.SellingAmount = null;
        arrived.PreRegisteredLot.UpdatedAt = DateTime.UtcNow;

        _db.PreRegisteredLots.Update(arrived.PreRegisteredLot);
    }

    await _db.SaveChangesAsync();
    await tx.CommitAsync();

    return auction.ToDto();
}

    }
}
