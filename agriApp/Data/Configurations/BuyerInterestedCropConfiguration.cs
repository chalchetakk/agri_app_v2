using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using agriApp.Entities.Stakeholders;

namespace agriApp.Data.Configurations
{
    public class BuyerInterestedCropConfiguration : IEntityTypeConfiguration<BuyerInterestedCrop>
    {
        public void Configure(EntityTypeBuilder<BuyerInterestedCrop> builder)
        {
            builder.ToTable("BuyerInterestedCrops");

            builder.HasKey(x => x.BuyerInterestedCropId);

            builder.HasOne(x => x.Buyer)
                .WithMany(b => b.InterestedCrops)
                .HasForeignKey(x => x.BuyerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.BuyerId, x.CropId })
                   .IsUnique();
        }
    }
}
