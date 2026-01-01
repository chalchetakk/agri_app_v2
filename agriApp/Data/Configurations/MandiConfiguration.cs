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


            // Optional but recommended
            builder.HasIndex(m => m.District);     
            // 🌱 Seed Mandis
            builder.HasData(
                new Mandi { MandiId = 1, MandiName = "Pune Marketyard Mandi", Location = "Pune" , District = "Pune" },
                new Mandi { MandiId = 2, MandiName = "Vashi Mandi", Location = "Mumbai" , District = "Navi Mumbai"},
                new Mandi { MandiId = 3, MandiName = "Cotton Market", Location = "Nagpur" , District = "Nagpur" }
            );
        }
    }
}
