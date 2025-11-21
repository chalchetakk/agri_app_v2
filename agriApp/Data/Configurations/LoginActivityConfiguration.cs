using agriApp.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations
{
    public class LoginActivityConfiguration : IEntityTypeConfiguration<LoginActivity>
    {
        public void Configure(EntityTypeBuilder<LoginActivity> builder)
        {
            // Table name
            builder.ToTable("LoginActivities");

            // Primary Key
            builder.HasKey(x => x.ActivityId);

            // Foreign Key → UserProfile
            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // LoginTime - required
            builder.Property(x => x.LoginTime)
                   .IsRequired();

            // IsSuccessful - required
            builder.Property(x => x.IsSuccessful)
                   .IsRequired();

            // FailureReason - optional (validated in entity)
            builder.Property(x => x.FailureReason)
                   .HasMaxLength(500);

            // IpAddress - optional
            builder.Property(x => x.IpAddress)
                   .HasMaxLength(50);

            // DeviceInfo - optional
            builder.Property(x => x.DeviceInfo)
                   .HasMaxLength(200);

            // Index for faster user-based queries
            builder.HasIndex(x => x.UserId);
        }
    }
}
