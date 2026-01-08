using agriApp.Entities.Market;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations
{
    public class MandiConfiguration : IEntityTypeConfiguration<Mandi>
    {
        public void Configure(EntityTypeBuilder<Mandi> builder)
        {
            builder.ToTable("Mandis");

            builder.HasKey(m => m.MandiId);

            builder.Property(m => m.MandiName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.Location)
                .IsRequired()
                .HasMaxLength(200);

            // ✅ NEW
            builder.Property(m => m.District)
                   .IsRequired()
                   .HasMaxLength(150);



            // 🔹 NEW COLUMNS
            builder.Property(m => m.MandiCode)
                   .HasMaxLength(64);

            builder.Property(m => m.AddressLine1)
                   .HasMaxLength(256);

            builder.Property(m => m.AddressLine2)
                   .HasMaxLength(256);

            builder.Property(m => m.WorkingDays)
                   .HasMaxLength(64);

            builder.Property(m => m.WeighingType)
                   .HasMaxLength(128);

            builder.Property(m => m.ContactPhone)
                   .HasMaxLength(32);

            builder.Property(m => m.ContactEmail)
                   .HasMaxLength(128);

            builder.Property(m => m.Website)
                   .HasMaxLength(256);
builder.Property(m => m.MandiCategoryId)
       .IsRequired(false);
            // 🔹 CATEGORY FK
            builder.HasOne(m => m.MandiCategory)
                   .WithMany()
                   .HasForeignKey(m => m.MandiCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 🔹 GEO FKs
            builder.HasOne(m => m.State)
                   .WithMany()
                   .HasForeignKey(m => m.StateId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(m => m.GeoDistrict)
                   .WithMany()
                   .HasForeignKey(m => m.DistrictId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(m => m.Taluka)
                   .WithMany()
                   .HasForeignKey(m => m.TalukaId)
                   .OnDelete(DeleteBehavior.SetNull);

            // 🔹 Indexes
            builder.HasIndex(m => m.MandiName);
            builder.HasIndex(m => m.DistrictId);
            builder.HasIndex(m => m.StateId);

            // Optional but recommended
            builder.HasIndex(m => m.District);     
            // 🌱 Seed Mandis
            // builder.HasData(
            //     new Mandi { MandiId = 1, MandiName = "Pune Marketyard Mandi", Location = "Pune" , District = "Pune" },
            //     new Mandi { MandiId = 2, MandiName = "Vashi Mandi", Location = "Mumbai" , District = "Navi Mumbai"},
            //     new Mandi { MandiId = 3, MandiName = "Cotton Market", Location = "Nagpur" , District = "Nagpur" }
            // );
        }
    }
}
