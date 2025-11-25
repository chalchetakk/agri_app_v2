using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using agriApp.Entities.Stakeholders;

namespace agriApp.Data.Configurations
{
    public class BuyerConfiguration : IEntityTypeConfiguration<Buyer>
    {
        public void Configure(EntityTypeBuilder<Buyer> builder)
        {
            builder.ToTable("Buyers");

            builder.HasKey(x => x.BuyerId);

            builder.Property(x => x.BuyerName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.BusinessName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.BusinessId).HasMaxLength(50);
            builder.Property(x => x.Location).HasMaxLength(200);
            builder.Property(x => x.ProfilePhotoUrl).HasMaxLength(500);

            builder.Property(x => x.CreatedAt).IsRequired();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.UserId);
        }
    }
}
