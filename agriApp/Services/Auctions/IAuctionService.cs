using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using agriApp.Entities.Auctions;
using agriApp.Entities.Lots;
using agriApp.Dtos.Auctions;

namespace agriApp.Services.Auctions
{
    public interface IAuctionService
    {
        Task<Auction> CreateAuctionAsync(CreateAuctionRequest dto);
Task<Auction> EditAuctionAsync(Guid auctionId, EditAuctionRequest dto);
Task<List<AuctionListItemDto>> GetAuctionsForMandiAsync(int mandiId);
Task<AuctionDetailDto?> GetAuctionByIdAsync(Guid auctionId);
Task<Auction> StartAuctionAsync(Guid auctionId, Guid officerId);
Task<Auction> EndAuctionAsync(Guid auctionId, Guid officerId);
Task<List<LiveAuctionLot>> GetLiveLotsForAuctionAsync(Guid auctionId);

    }
}
