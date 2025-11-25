using agriApp.Entities.Stakeholders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using agriApp.Entities.Stakeholders;

namespace agriApp.Data.Configurations
{
    public class FarmerConfiguration : IEntityTypeConfiguration<Farmer>
    {
        public void Configure(EntityTypeBuilder<Farmer> builder)
        {
            // Table Name
            builder.ToTable("Farmers");

            // Primary Key
            builder.HasKey(x => x.FarmerId);

            // FarmerName - required
            builder.Property(x => x.FarmerName)
                .IsRequired()
                .HasMaxLength(200);

            // Location - optional
            builder.Property(x => x.Location)
                .HasMaxLength(200);

            // Profile photo URL - optional
            builder.Property(x => x.ProfilePhotoUrl)
                .HasMaxLength(500);

            // FK → UserProfile
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // CreatedAt - required
            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // UpdatedAt - optional
            builder.Property(x => x.UpdatedAt);

            // Navigation collections handled automatically
        }
    }
}
