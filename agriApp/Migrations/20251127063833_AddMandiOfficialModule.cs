using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace agriApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMandiOfficialModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mandis",
                columns: table => new
                {
                    MandiId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MandiName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mandis", x => x.MandiId);
                });

            migrationBuilder.CreateTable(
                name: "OfficialRoles",
                columns: table => new
                {
                    OfficialRoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    OfficialRoleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RoleCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficialRoles", x => x.OfficialRoleId);
                });

            migrationBuilder.CreateTable(
                name: "MandiOfficials",
                columns: table => new
                {
                    OfficialId = table.Column<Guid>(type: "uuid", nullable: false),
                    OfficialName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EmployeeId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    MandiId = table.Column<int>(type: "integer", nullable: false),
                    OfficialRoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MandiOfficials", x => x.OfficialId);
                    table.ForeignKey(
                        name: "FK_MandiOfficials_Mandis_MandiId",
                        column: x => x.MandiId,
                        principalTable: "Mandis",
                        principalColumn: "MandiId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MandiOfficials_OfficialRoles_OfficialRoleId",
                        column: x => x.OfficialRoleId,
                        principalTable: "OfficialRoles",
                        principalColumn: "OfficialRoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MandiOfficials_UserProfiles_UserId",
                        column: x => x.UserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MandiOfficials_MandiId",
                table: "MandiOfficials",
                column: "MandiId");

            migrationBuilder.CreateIndex(
                name: "IX_MandiOfficials_OfficialRoleId",
                table: "MandiOfficials",
                column: "OfficialRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MandiOfficials_UserId",
                table: "MandiOfficials",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MandiOfficials");

            migrationBuilder.DropTable(
                name: "Mandis");

            migrationBuilder.DropTable(
                name: "OfficialRoles");
        }
    }
}
