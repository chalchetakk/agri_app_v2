using agriApp.Entities.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations.Geography
{
    public class GeoTalukaConfiguration : IEntityTypeConfiguration<GeoTaluka>
    {
        public void Configure(EntityTypeBuilder<GeoTaluka> builder)
        {
            builder.ToTable("geo_taluka");

            builder.HasKey(t => t.TalukaId);

            builder.Property(t => t.TalukaName)
                   .IsRequired()
                   .HasMaxLength(128);

            builder.Property(t => t.TalukaCode)
                   .HasMaxLength(32);

            builder.HasOne(t => t.District)
                   .WithMany(d => d.Talukas)
                   .HasForeignKey(t => t.DistrictId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(t => new { t.DistrictId, t.TalukaName })
                   .IsUnique();
        }
    }
}
