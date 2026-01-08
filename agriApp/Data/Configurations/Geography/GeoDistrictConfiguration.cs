using agriApp.Entities.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations.Geography
{
    public class GeoDistrictConfiguration : IEntityTypeConfiguration<GeoDistrict>
    {
        public void Configure(EntityTypeBuilder<GeoDistrict> builder)
        {
            builder.ToTable("geo_district");

            builder.HasKey(d => d.DistrictId);

            builder.Property(d => d.DistrictName)
                   .IsRequired()
                   .HasMaxLength(128);

            builder.Property(d => d.DistrictCode)
                   .HasMaxLength(32);

            builder.HasOne(d => d.State)
                   .WithMany(s => s.Districts)
                   .HasForeignKey(d => d.StateId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(d => new { d.StateId, d.DistrictName })
                   .IsUnique();
        }
    }
}
