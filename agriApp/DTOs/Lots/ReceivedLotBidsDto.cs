using System;
using System.Collections.Generic;

namespace agriApp.Dtos.Lots
{
    public class ReceivedLotBidsDto
    {
        public string PreLotId { get; set; } = default!;
        public string CropName { get; set; } = default!;
        public string MandiName { get; set; } = default!;
        public List<BidListItemDto> Bids { get; set; } = new();
    }
}
