using agriApp.Entities.Lots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.EntityConfigurations
{
    public class BuyerInterestLotConfiguration : IEntityTypeConfiguration<BuyerInterestLot>
    {
        public void Configure(EntityTypeBuilder<BuyerInterestLot> builder)
        {
            builder.ToTable("BuyerInterestLots");

            builder.HasKey(b => b.BuyerInterestLotId);

            // Unique constraint: (PreLotId, BuyerId)
            builder.HasIndex(b => new { b.PreLotId, b.BuyerId })
                   .IsUnique();

            // FK → PreRegisteredLot
            builder.HasOne(b => b.PreRegisteredLot)
                   .WithMany()  
                   .HasForeignKey(b => b.PreLotId)
                   .HasPrincipalKey(p => p.PreLotId)
                   .OnDelete(DeleteBehavior.Cascade);

            // FK → Buyer (correct navigation)
            builder.HasOne(b => b.Buyer)
                   .WithMany(bu => bu.BuyerInterestLots)   // ✅ FIX HERE
                   .HasForeignKey(b => b.BuyerId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Column types
            builder.Property(b => b.BuyerBidAmount)
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            builder.Property(b => b.Status)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(b => b.CreatedAt)
                   .IsRequired();

            builder.Property(b => b.UpdatedAt)
                   .IsRequired();
        }
    }
}
