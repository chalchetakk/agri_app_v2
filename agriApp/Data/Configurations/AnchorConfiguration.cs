using agriApp.Entities.Stakeholders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations
{
    public class AnchorConfiguration : IEntityTypeConfiguration<Anchor>
    {
        public void Configure(EntityTypeBuilder<Anchor> builder)
        {
            // Table Name
            builder.ToTable("Anchors");

            // Primary Key
            builder.HasKey(a => a.AnchorId);

            // FK -> UserProfile
            builder
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Required Fields (with varchar lengths)
            builder.Property(a => a.CompanyName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.RegistrationNumber)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.CompanyAddress)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.ContactPersonName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.ContactPersonNum)
                .IsRequired()
                .HasMaxLength(10);

            // Optional Fields
            builder.Property(a => a.GSTNumber)
                .HasMaxLength(20);

            builder.Property(a => a.BusinessDescription)
                .HasColumnType("text");

            // Estimated Farmers > 0 (DB cannot enforce >0, but required)
            builder.Property(a => a.EstimatedFarmersNum)
                .IsRequired();

            // Timestamps
            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.UpdatedAt);
        }
    }
}
