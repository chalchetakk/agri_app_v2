using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using agriApp.Data;
using agriApp.Dtos.Auctions;
using agriApp.Entities.Auctions;
using agriApp.Entities.Lots;
using Microsoft.EntityFrameworkCore;

namespace agriApp.Services.Auctions
{
    public class AuctionService : IAuctionService
    {
        private readonly AgriDbContext _db;

        public AuctionService(AgriDbContext db)
        {
            _db = db;
        }

        // ------------------------------------------------------------
        // 1) CREATE AUCTION (Manager)
        // ------------------------------------------------------------
        public async Task<Auction> CreateAuctionAsync(CreateAuctionRequest dto)
        {
            // Validate mandi
            var mandi = await _db.Mandis.FindAsync(dto.MandiId)
                ?? throw new Exception("Invalid MandiId.");

            // Validate crop
            var crop = await _db.Crops.FindAsync(dto.CropId)
                ?? throw new Exception("Invalid CropId.");

            // Validate assigned officer
            var officer = await _db.MandiOfficials
                .FirstOrDefaultAsync(m => m.OfficialId == dto.AssignedOfficerId)
                ?? throw new Exception("Invalid AssignedOfficerId.");

            if (officer.MandiId != dto.MandiId)
                throw new Exception("Officer does not belong to this mandi.");

// ⭐ NEW VALIDATION — REQUIRED!
    var createdBy = await _db.MandiOfficials
        .FirstOrDefaultAsync(m => m.OfficialId == dto.CreatedByOfficialId)
        ?? throw new Exception("Invalid CreatedByOfficialId.");

    if (createdBy.MandiId != dto.MandiId)
        throw new Exception("Creator does not belong to this mandi.");

            // Create Auction entity
            var auction = dto.ToEntity();

            _db.Auctions.Add(auction);
            await _db.SaveChangesAsync();

            return auction;
        }

        // ------------------------------------------------------------
        // 2) EDIT AUCTION (Manager)
        // ------------------------------------------------------------
        public async Task<Auction> EditAuctionAsync(Guid auctionId, EditAuctionRequest dto)
        {
            var auction = await _db.Auctions.FindAsync(auctionId)
                ?? throw new Exception("Auction not found.");

            if (auction.Status != "scheduled")
                throw new Exception("Only scheduled auctions can be edited.");

            if (dto.CropId != null)
            {
                var crop = await _db.Crops.FindAsync(dto.CropId.Value)
                    ?? throw new Exception("Invalid crop.");
                auction.CropId = dto.CropId.Value;
            }

            if (dto.ScheduledAt != null)
                auction.ScheduledAt = dto.ScheduledAt.Value;

            if (dto.AssignedOfficerId != null)
            {
                var officer = await _db.MandiOfficials
                    .FirstOrDefaultAsync(o => o.OfficialId == dto.AssignedOfficerId.Value)
                    ?? throw new Exception("Invalid assigned officer.");

                if (officer.MandiId != auction.MandiId)
                    throw new Exception("Officer does not belong to this mandi.");

                auction.AssignedOfficerId = dto.AssignedOfficerId.Value;
            }

            auction.UpdatedAt = DateTime.UtcNow;

            _db.Auctions.Update(auction);
            await _db.SaveChangesAsync();

            return auction;
        }

        // ------------------------------------------------------------
        // 3) GET ALL AUCTIONS FOR A MANDI
        // ------------------------------------------------------------
        public async Task<List<AuctionListItemDto>> GetAuctionsForMandiAsync(int mandiId, Guid ? officerId = null)
        {
            var query =  _db.Auctions
                .Include(a => a.Crop)
                .Include(a => a.Mandi)
                .Include(a => a.AssignedOfficer)
                .Where(a => a.MandiId == mandiId);
                // ⭐ Apply officer filtering only when officerId is provided
    if (officerId.HasValue)
    {
        query = query.Where(a => a.AssignedOfficerId == officerId.Value);
    }

    return await query
                .OrderByDescending(a => a.ScheduledAt)
                .Select(a => new AuctionListItemDto
                {
                    AuctionId = a.AuctionId,
                    MandiId = a.MandiId,
                    MandiName = a.Mandi!.MandiName,
                    CropId = a.CropId,
                    CropName = a.Crop!.CropName,
                    AssignedOfficerName = a.AssignedOfficer!.OfficialName,
                    Status = a.Status,
                    ScheduledAt = a.ScheduledAt,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();
        }

        // ------------------------------------------------------------
        // 4) GET AUCTION DETAIL
        // ------------------------------------------------------------
        public async Task<AuctionDetailDto?> GetAuctionByIdAsync(Guid auctionId)
        {
            var auction = await _db.Auctions
                .Include(a => a.Crop)
                .Include(a => a.Mandi)
                .Include(a => a.AssignedOfficer)
                .Include(a => a.CreatedByOfficial)
                .Include(a => a.LiveAuctionLots)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (auction == null) return null;

            return new AuctionDetailDto
            {
                AuctionId = auction.AuctionId,
                MandiId = auction.MandiId,
                MandiName = auction.Mandi!.MandiName,
                CropId = auction.CropId,
                CropName = auction.Crop!.CropName,
                AssignedOfficerId = auction.AssignedOfficerId,
                AssignedOfficerName = auction.AssignedOfficer!.OfficialName,
                CreatedByOfficialId = auction.CreatedByOfficialId,
                CreatedByOfficialName = auction.CreatedByOfficial!.OfficialName,
                Status = auction.Status,
                ScheduledAt = auction.ScheduledAt,
                CreatedAt = auction.CreatedAt,
                LiveLots = auction.LiveAuctionLots.ToList()
            };
        }

        // ------------------------------------------------------------
        // 5) START AUCTION (Officer)
        // ------------------------------------------------------------
        public async Task<Auction> StartAuctionAsync(Guid auctionId, Guid officerId)
        {
            var auction = await _db.Auctions.FindAsync(auctionId)
                ?? throw new Exception("Auction not found.");

            if (auction.AssignedOfficerId != officerId)
                throw new Exception("You are not assigned to this auction.");

            if (auction.Status != "scheduled")
                throw new Exception("Auction can only start from status 'scheduled'.");

            auction.Status = "started";
            auction.UpdatedAt = DateTime.UtcNow;

            _db.Auctions.Update(auction);
            await _db.SaveChangesAsync();

            return auction;
        }

        // ------------------------------------------------------------
        // 6) END AUCTION (Officer) + AUTO-MARK UNSOLD
        // ------------------------------------------------------------
        public async Task<Auction> EndAuctionAsync(Guid auctionId, Guid officerId)
{
    // Step 1: Load auction safely (same method as StartAuction)
    var auction = await _db.Auctions.FindAsync(auctionId)
        ?? throw new Exception("Auction not found.");
// Console.WriteLine("AssignedOfficerId: " + auction.AssignedOfficerId);
// Console.WriteLine("OfficerId received: " + officerId);


    if (auction.AssignedOfficerId != officerId)
        throw new Exception("You are not assigned to this auction.");

    if (auction.Status != "started")
        throw new Exception("Auction must be in 'started' state to end.");

    // Step 2: Load LiveAuctionLots separately (as they are not navigation-loaded by FindAsync)
    var liveLots = await _db.LiveAuctionLots
        .Where(x => x.AuctionId == auctionId)
        .ToListAsync();

    // Step 3: Mark auction as ended
    auction.Status = "ended";
    auction.UpdatedAt = DateTime.UtcNow;

    // Step 4: Auto mark unsold lots
    foreach (var lot in liveLots)
    {
        if (lot.AuctionStatus == "pending")
        {
            lot.AuctionStatus = "unsold";
            lot.UpdatedAt = DateTime.UtcNow;

            // Update ArrivedLot & PreRegisteredLot
            var arrived = await _db.ArrivedLots
                .Include(a => a.PreRegisteredLot)
                .FirstOrDefaultAsync(a => a.ArrivedLotId == lot.ArrivedLotId);

            arrived!.Status = "unsold";
            arrived.UpdatedAt = DateTime.UtcNow;

            if (arrived.PreRegisteredLot != null)
            {
                arrived.PreRegisteredLot.Status = "unsold";
                arrived.PreRegisteredLot.UpdatedAt = DateTime.UtcNow;
            }
        }
    }

    await _db.SaveChangesAsync();
    return auction;
}

        // ------------------------------------------------------------
        // 7) GET LIVE LOTS FOR AUCTION
        // ------------------------------------------------------------
        public async Task<List<LiveAuctionLot>> GetLiveLotsForAuctionAsync(Guid auctionId)
        {
            return await _db.LiveAuctionLots
                .Where(l => l.AuctionId == auctionId)
                .OrderBy(l => l.CreatedAt)
                .ToListAsync();
        }
    }
}
