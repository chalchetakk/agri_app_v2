using agriApp.Entities.Stakeholders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations
{
    public class AnchorFarmerConfiguration : IEntityTypeConfiguration<AnchorFarmer>
    {
        public void Configure(EntityTypeBuilder<AnchorFarmer> builder)
        {
            builder.ToTable("AnchorFarmers");

            builder.HasKey(x => x.AnchorFarmerId);

            // FK → Anchor
            builder.HasOne(x => x.Anchor)
                .WithMany()
                .HasForeignKey(x => x.AnchorId)
                .OnDelete(DeleteBehavior.Cascade);

            // FK → Farmer
            builder.HasOne(x => x.Farmer)
                .WithMany()
                .HasForeignKey(x => x.FarmerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Only ONE anchor can own a farmer (your business rule)
            builder.HasIndex(x => x.FarmerId)
                .IsUnique();

            builder.Property(x => x.CreatedAt)
                .IsRequired();
        }
    }
}
