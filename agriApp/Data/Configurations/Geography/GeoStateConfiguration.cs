using agriApp.Entities.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations.Geography
{
    public class GeoStateConfiguration : IEntityTypeConfiguration<GeoState>
    {
        public void Configure(EntityTypeBuilder<GeoState> builder)
        {
            builder.ToTable("geo_state");

            builder.HasKey(s => s.StateId);

            builder.Property(s => s.StateName)
                   .IsRequired()
                   .HasMaxLength(128);

            builder.Property(s => s.StateCode)
                   .HasMaxLength(32);

            builder.HasIndex(s => s.StateName)
                   .IsUnique();
                   builder.HasData(
    new GeoState
    {
        StateId = 1,
        StateName = "Maharashtra",
        StateCode = "MH",
        IsActive = true
    }
);

        }
    }
}
