using agriApp.Entities.Stakeholders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations
{
    public class SellerInterestedCropConfiguration : IEntityTypeConfiguration<SellerInterestedCrop>
    {
        public void Configure(EntityTypeBuilder<SellerInterestedCrop> builder)
        {
            builder.ToTable("SellerInterestedCrops");

            builder.HasKey(x => x.SellerInterestedCropId);

            builder.HasIndex(x => new { x.SellerId, x.CropId })
                   .IsUnique();
        }
    }
}
