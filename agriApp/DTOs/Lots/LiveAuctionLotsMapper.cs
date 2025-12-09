using agriApp.Entities.Lots;

namespace agriApp.Dtos.Lots
{
    public static class LiveAuctionLotsMapper
    {
        public static LiveAuctionLotDto ToDto(this LiveAuctionLot entity)
        {
            return new LiveAuctionLotDto
            {
                LiveAuctionLotId = entity.LiveAuctionLotId,
                AuctionId = entity.AuctionId,
                ArrivedLotId = entity.ArrivedLotId,

                LotOwnerName = entity.ArrivedLot?.LotOwnerName,
                MobileNum = entity.ArrivedLot?.MobileNum,
                CropName = entity.ArrivedLot?.Crop?.CropName,
                Grade = entity.ArrivedLot?.Grade,
                Quantity = entity.ArrivedLot?.Quantity ?? 0,

                BuyerName = entity.BuyerName,
                BuyerMobile = entity.BuyerMobile,

                AuctionStatus = entity.AuctionStatus
            };
        }
    }
}
