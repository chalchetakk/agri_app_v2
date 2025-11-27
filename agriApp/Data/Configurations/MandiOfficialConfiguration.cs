using agriApp.Entities.Stakeholders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using agriApp.Entities.Stakeholders;

namespace agriApp.Data.Configurations
{
    public class MandiOfficialConfiguration : IEntityTypeConfiguration<MandiOfficial>
    {
        public void Configure(EntityTypeBuilder<MandiOfficial> builder)
        {
            builder.ToTable("MandiOfficials");

            builder.HasKey(o => o.OfficialId);

            builder.Property(o => o.OfficialName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(o => o.EmployeeId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(o => o.CreatedAt)
                .IsRequired();

            builder.Property(o => o.UpdatedAt);

            // FK → Mandis
            builder.HasOne(o => o.Mandi)
                .WithMany(m => m.Officials)
                .HasForeignKey(o => o.MandiId)
                .OnDelete(DeleteBehavior.Restrict);

            // FK → OfficialRole
            builder.HasOne(o => o.Role)
                .WithMany(r => r.MandiOfficials)
                .HasForeignKey(o => o.OfficialRoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
