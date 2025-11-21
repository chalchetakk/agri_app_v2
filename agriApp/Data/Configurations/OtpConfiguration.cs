using agriApp.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations
{
    public class OtpConfiguration : IEntityTypeConfiguration<Otp>
    {
        public void Configure(EntityTypeBuilder<Otp> builder)
        {
            // Table name
            builder.ToTable("Otps");

            // Primary Key
            builder.HasKey(x => x.OtpId);

            // Foreign Key → UserProfile
            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // OtpHash - required
            builder.Property(x => x.OtpHash)
                   .IsRequired();

            // Expiry
            builder.Property(x => x.ExpireAt)
                   .IsRequired();

            // Usage tracking
            builder.Property(x => x.IsUsed)
                   .IsRequired();

            builder.Property(x => x.UsedAt);

            // CreatedAt
            builder.Property(x => x.CreatedAt)
                   .IsRequired();

            // Index for faster queries
            builder.HasIndex(x => x.UserId);
        }
    }
}
