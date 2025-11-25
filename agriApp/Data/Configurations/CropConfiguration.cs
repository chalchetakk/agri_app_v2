using agriApp.Entities.Market;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations
{
    public class CropConfiguration : IEntityTypeConfiguration<Crop>
    {
        public void Configure(EntityTypeBuilder<Crop> builder)
        {
            builder.ToTable("Crops");

            builder.HasKey(x => x.CropId);

            builder.Property(x => x.CropName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(x => x.CropName)
                   .IsUnique();

            builder.Property(x => x.Grade)
                   .HasMaxLength(50)
                   .IsRequired(false);

            builder.Property(x => x.CreatedAt)
                   .IsRequired();
        }
    }
}
