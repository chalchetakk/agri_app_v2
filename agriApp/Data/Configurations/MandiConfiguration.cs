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
            
            // 🌱 Seed Mandis
            builder.HasData(
                new Mandi { MandiId = 1, MandiName = "Pune Marketyard Mandi", Location = "Pune" },
                new Mandi { MandiId = 2, MandiName = "Vashi Mandi", Location = "Mumbai" },
                new Mandi { MandiId = 3, MandiName = "Cotton Market", Location = "Nagpur" }
            );
        }
    }
}
