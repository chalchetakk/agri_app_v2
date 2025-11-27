using agriApp.Entities.Stakeholders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agriApp.Data.Configurations
{
    public class OfficialRoleConfiguration : IEntityTypeConfiguration<OfficialRole>
    {
        public void Configure(EntityTypeBuilder<OfficialRole> builder)
        {
            builder.ToTable("OfficialRoles");

            builder.HasKey(r => r.OfficialRoleId);

            builder.Property(r => r.OfficialRoleName)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
