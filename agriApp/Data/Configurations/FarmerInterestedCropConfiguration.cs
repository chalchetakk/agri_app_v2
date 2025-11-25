using agriApp.Entities.Stakeholders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using agriApp.Entities.Stakeholders;

namespace agriApp.Data.Configurations
{
    public class FarmerInterestedCropConfiguration : IEntityTypeConfiguration<FarmerInterestedCrop>
    {
        public void Configure(EntityTypeBuilder<FarmerInterestedCrop> builder)
        {
            builder.ToTable("FarmerInterestedCrops");

            builder.HasKey(x => x.FarmerInterestedCropId);

            // Unique constraint: farmerId + cropId
            builder.HasIndex(x => new { x.FarmerId, x.CropId })
                .IsUnique();

            // Farmer FK
            builder.HasOne(x => x.Farmer)
                .WithMany(f => f.InterestedCrops)
                .HasForeignKey(x => x.FarmerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Properties
            builder.Property(x => x.CropId)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt);
        }
    }
}
