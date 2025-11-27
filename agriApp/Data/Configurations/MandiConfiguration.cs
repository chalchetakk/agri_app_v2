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
        }
    }
}
