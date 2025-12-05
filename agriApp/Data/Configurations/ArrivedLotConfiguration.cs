using agriApp.Entities.Lots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations
{
    public class ArrivedLotConfiguration : IEntityTypeConfiguration<ArrivedLot>
    {
        public void Configure(EntityTypeBuilder<ArrivedLot> builder)
        {
            builder.ToTable("ArrivedLots");

            builder.HasKey(a => a.ArrivedLotId);

            // -----------------------------
            // Basic Fields
            // -----------------------------
            builder.Property(a => a.LotOwnerRole)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(a => a.LotOwnerName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.MobileNum)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(a => a.QrCodeUrl)
                   .IsRequired();

            builder.Property(a => a.Status)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(a => a.CreatedAt)
                   .IsRequired();

            builder.Property(a => a.UpdatedAt)
                   .IsRequired();

            // -----------------------------
            // Relationships
            // -----------------------------

            // PreRegisteredLot — optional but UNIQUE
            builder.HasOne(a => a.PreRegisteredLot)
                   .WithMany()
                   .HasForeignKey(a => a.PreLotId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Make PreLotId UNIQUE
            builder.HasIndex(a => a.PreLotId)
                   .IsUnique();

            // Farmer (optional)
            builder.HasOne(a => a.Farmer)
                   .WithMany()
                   .HasForeignKey(a => a.FarmerId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Seller (optional)
            builder.HasOne(a => a.Seller)
                   .WithMany()
                   .HasForeignKey(a => a.SellerId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Mandi (required)
            builder.HasOne(a => a.Mandi)
                   .WithMany()
                   .HasForeignKey(a => a.MandiId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Mandi Officer (required)
            builder.HasOne(a => a.MandiOfficer)
                   .WithMany()
                   .HasForeignKey(a => a.MandiOfficerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Crop (required)
            builder.HasOne(a => a.Crop)
                   .WithMany()
                   .HasForeignKey(a => a.CropId)
                   .OnDelete(DeleteBehavior.Restrict);

            // -----------------------------
            // Additional indexes
            // -----------------------------
            builder.HasIndex(a => a.Status);
            builder.HasIndex(a => a.MandiId);
            builder.HasIndex(a => a.MobileNum);
        }
    }
}
