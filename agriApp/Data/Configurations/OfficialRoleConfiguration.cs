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

            // 🌱 Seed Roles
            builder.HasData(
                new OfficialRole
                {
                    OfficialRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    OfficialRoleName = "Mandi Officer",
                    RoleCode = "OFFICER"
                },
                new OfficialRole
                {
                    OfficialRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    OfficialRoleName = "Mandi Manager",
                    RoleCode = "MANAGER"
                },
                new OfficialRole
                {
                    OfficialRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    OfficialRoleName = "Mandi Approver",
                    RoleCode = "APPROVER"
                }
            );
        }
        
    }
}
