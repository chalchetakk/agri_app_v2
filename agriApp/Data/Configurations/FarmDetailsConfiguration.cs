using agriApp.Entities.Stakeholders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations
{
    public class FarmDetailsConfiguration : IEntityTypeConfiguration<FarmDetails>
    {
        public void Configure(EntityTypeBuilder<FarmDetails> builder)
        {
            builder.ToTable("FarmDetails");

            builder.HasKey(x => x.FarmId);

            // FK → Farmer
            builder.HasOne(x => x.Farmer)
                .WithMany(f => f.FarmDetails)
                .HasForeignKey(x => x.FarmerId)
                .OnDelete(DeleteBehavior.Cascade);

            // FarmLocation - required
            builder.Property(x => x.FarmLocation)
                .IsRequired()
                .HasMaxLength(200);

            // PrimaryCrop - required
            builder.Property(x => x.PrimaryCrop)
                .IsRequired()
                .HasMaxLength(100);

            // FarmSize - required
            builder.Property(x => x.FarmSize)
                .IsRequired();

            // timestamps
            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt);
        }
    }
}
