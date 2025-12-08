using agriApp.Entities.Auctions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations
{
    public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.ToTable("Auctions");

            // Primary Key
            builder.HasKey(a => a.AuctionId);

            // Required fields
            builder.Property(a => a.Status)
                .IsRequired()
                .HasMaxLength(20); // scheduled | started | ended | cancelled

            builder.Property(a => a.ScheduledAt)
                .IsRequired();

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.UpdatedAt)
                .IsRequired();

            // ---------------------------
            // Foreign Keys
            // ---------------------------

            // FK → Mandi (required)
            builder.HasOne(a => a.Mandi)
                .WithMany(m => m.Auctions)     // You MUST add List<Auction> in Mandi
                .HasForeignKey(a => a.MandiId)
                .OnDelete(DeleteBehavior.Restrict);

            // FK → Crop (required)
            builder.HasOne(a => a.Crop)
                .WithMany()
                .HasForeignKey(a => a.CropId)
                .OnDelete(DeleteBehavior.Restrict);

            // FK → Assigned Officer (required)
            builder.HasOne(a => a.AssignedOfficer)
                .WithMany()
                .HasForeignKey(a => a.AssignedOfficerId)
                .OnDelete(DeleteBehavior.Restrict);

            // FK → Created By (required)
            builder.HasOne(a => a.CreatedByOfficial)
                .WithMany()
                .HasForeignKey(a => a.CreatedByOfficialId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------
            // Navigation: LiveAuctionLots
            // ----------------------------
            builder.HasMany(a => a.LiveAuctionLots)
                .WithOne(l => l.Auction)
                .HasForeignKey(l => l.AuctionId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------
            // Indexes (performance must-haves)
            // ---------------------------
            builder.HasIndex(a => a.MandiId);
            builder.HasIndex(a => a.CropId);
            builder.HasIndex(a => a.AssignedOfficerId);
            builder.HasIndex(a => a.CreatedByOfficialId);
            builder.HasIndex(a => a.Status);
            builder.HasIndex(a => a.ScheduledAt);
        }
    }
}
