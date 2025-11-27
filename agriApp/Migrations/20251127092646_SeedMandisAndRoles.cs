using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace agriApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedMandisAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Mandis",
                columns: new[] { "MandiId", "Location", "MandiName" },
                values: new object[,]
                {
                    { 1, "Pune", "Pune Marketyard Mandi" },
                    { 2, "Mumbai", "Vashi Mandi" },
                    { 3, "Nagpur", "Cotton Market" }
                });

            migrationBuilder.InsertData(
                table: "OfficialRoles",
                columns: new[] { "OfficialRoleId", "OfficialRoleName", "RoleCode" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Mandi Officer", "OFFICER" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Mandi Manager", "MANAGER" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Mandi Approver", "APPROVER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Mandis",
                keyColumn: "MandiId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Mandis",
                keyColumn: "MandiId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Mandis",
                keyColumn: "MandiId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OfficialRoles",
                keyColumn: "OfficialRoleId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "OfficialRoles",
                keyColumn: "OfficialRoleId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "OfficialRoles",
                keyColumn: "OfficialRoleId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));
        }
    }
}
