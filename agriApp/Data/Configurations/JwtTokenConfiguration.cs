using agriApp.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations
{
    public class JwtTokenConfiguration : IEntityTypeConfiguration<JwtToken>
    {
        public void Configure(EntityTypeBuilder<JwtToken> builder)
        {
            // Table name
            builder.ToTable("JwtTokens");

            // Primary Key
            builder.HasKey(x => x.JwtTokenId);

            // FK → UserProfile
            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Self-referencing FK for token rotation chain
            builder.HasOne<JwtToken>()
                   .WithMany()
                   .HasForeignKey(x => x.ReplacedByTokenId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Access token fields
            builder.Property(x => x.AccessTokenJti)
                   .IsRequired();

            builder.Property(x => x.AccessTokenIssuedAt)
                   .IsRequired();

            builder.Property(x => x.AccessTokenExpiresAt)
                   .IsRequired();

            // Refresh token fields
            builder.Property(x => x.RefreshTokenHash)
                   .IsRequired();

            builder.Property(x => x.RefreshTokenIssuedAt)
                   .IsRequired();

            builder.Property(x => x.RefreshTokenExpiresAt)
                   .IsRequired();

            // Revocation
            builder.Property(x => x.IsRevoked)
                   .IsRequired();

            builder.Property(x => x.RevokedAt);

            // Metadata
            builder.Property(x => x.DeviceInfo)
                   .HasMaxLength(200);

            builder.Property(x => x.IpAddress)
                   .HasMaxLength(50);

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.RefreshTokenHash);
            builder.HasIndex(x => x.AccessTokenJti);
        }
    }
}
