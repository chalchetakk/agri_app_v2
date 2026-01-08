using agriApp.Entities.Lookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations.Lookups
{
    public class MandiCategoryConfiguration : IEntityTypeConfiguration<MandiCategory>
    {
        public void Configure(EntityTypeBuilder<MandiCategory> builder)
        {
            builder.ToTable("lookup_mandi_category");

            builder.HasKey(c => c.MandiCategoryId);

            builder.Property(c => c.CategoryName)
                   .IsRequired()
                   .HasMaxLength(64);

builder.Property(c => c.Description)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.HasIndex(c => c.CategoryName)
                   .IsUnique();

            // 🌱 Seed data (small & static)
            builder.HasData(
                new MandiCategory
                {
                    MandiCategoryId = 1,
                    CategoryName = "APMC",
                    Description = "Agricultural Produce Market Committee (regulated)",
                    IsActive = true
                },
                new MandiCategory
                {
                    MandiCategoryId = 2,
                    CategoryName = "eNAM",
                     Description = "Electronic National Agriculture Market",
                    IsActive = true
                },
                new MandiCategory
                {
                    MandiCategoryId = 3,
                    CategoryName = "Private",
                    Description = "Private marketplace or yard",
                    IsActive = true
                },
                new MandiCategory
                {
                    MandiCategoryId = 4,
                    CategoryName = "Cooperative",
                    Description = "Farmer cooperative market",
                    IsActive = true
                },
                new MandiCategory
                {
                    MandiCategoryId = 5,
                    CategoryName = "Terminal",
                    Description = "Terminal market",
                    IsActive = true
                }
            );
        }
    }
}
