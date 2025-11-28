using agriApp.Entities.Lots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations
{
    public class PreRegisteredLotConfiguration : IEntityTypeConfiguration<PreRegisteredLot>
    {
        public void Configure(EntityTypeBuilder<PreRegisteredLot> builder)
        {
            builder.ToTable("PreRegisteredLots");

            // 🔹 Primary Key → 10-digit string ID
            builder.HasKey(l => l.PreLotId);

            builder.Property(l => l.PreLotId)
                   .IsRequired()
                   .HasMaxLength(10);

            // 🔹 Owner: Farmer (nullable)
            builder.HasOne(l => l.Farmer)
                   .WithMany(f => f.Lots)
                   .HasForeignKey(l => l.FarmerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Owner: Seller (nullable)
            builder.HasOne(l => l.Seller)
                   .WithMany(s => s.Lots)
                   .HasForeignKey(l => l.SellerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ❗ Ensure BOTH FarmerId & SellerId are not filled simultaneously.
            // (We enforce this later in service, EF cannot do cross-property validation.)

            // 🔹 Crop FK
            builder.HasOne(l => l.Crop)
                   .WithMany(c => c.Lots)
                   .HasForeignKey(l => l.CropId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Mandi FK
            builder.HasOne(l => l.Mandi)
                   .WithMany(m => m.Lots)
                   .HasForeignKey(l => l.MandiId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Required Fields
            builder.Property(l => l.Status)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(l => l.Grade)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(l => l.Quantity)
                   .IsRequired();

            builder.Property(l => l.ExpectedArrivalDate)
                   .IsRequired();

            builder.Property(l => l.LotImageUrl)
                   .HasMaxLength(500);

            builder.Property(l => l.QrCodeUrl)
                   .HasMaxLength(500)
                   .IsRequired(false); 

            // 🔹 Timestamps
            builder.Property(l => l.CreatedAt)
                   .IsRequired();

            builder.Property(l => l.UpdatedAt);
        }
    }
}
