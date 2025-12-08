using agriApp.Entities.Lots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations
{
    public class LiveAuctionLotConfiguration : IEntityTypeConfiguration<LiveAuctionLot>
    {
        public void Configure(EntityTypeBuilder<LiveAuctionLot> builder)
        {
            builder.ToTable("LiveAuctionLots");

            builder.HasKey(x => x.LiveAuctionLotId);

            builder.Property(x => x.AuctionStatus)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            builder.Property(x => x.BuyerName).HasMaxLength(150);
            builder.Property(x => x.BuyerMobile).HasMaxLength(10);

            // FK → ArrivedLot
            builder.HasOne(x => x.ArrivedLot)
                .WithMany()
                .HasForeignKey(x => x.ArrivedLotId)
                .OnDelete(DeleteBehavior.Cascade);

            // FK → Buyer (optional)
            builder.HasOne(x => x.Buyer)
                .WithMany()
                .HasForeignKey(x => x.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------------------------
            // ⭐ NEW — FK → Auction (optional)
            // -------------------------------------------
            builder.HasOne(x => x.Auction)
                .WithMany(a => a.LiveAuctionLots)
                .HasForeignKey(x => x.AuctionId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(x => x.ArrivedLotId);
            builder.HasIndex(x => x.AuctionStatus);
            builder.HasIndex(x => x.BuyerId);
            builder.HasIndex(x => x.AuctionId);  // NEW INDEX
        }
    }
}
