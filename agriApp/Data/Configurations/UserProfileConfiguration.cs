using agriApp.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            // Table name
            builder.ToTable("UserProfiles");

            // Primary Key
            builder.HasKey(x => x.UserProfileId);

            // MobileNumber - required, unique, max length 10
            builder.Property(x => x.MobileNumber)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.HasIndex(x => x.MobileNumber)
                   .IsUnique();

            // PreferredLanguage - optional, max length 10
            builder.Property(x => x.PreferredLanguage)
                   .HasMaxLength(10);

            // IsVerified
            builder.Property(x => x.IsVerified)
                   .IsRequired();

            // LastLoginAt - optional
            builder.Property(x => x.LastLoginAt);

            // Timestamps
            builder.Property(x => x.CreationTime)
                   .IsRequired();

            builder.Property(x => x.LastModificationTime);

            // No extra relationships defined here
            // (OTP, JwtToken, LoginActivity configs will define their FK)
        }
    }
}
