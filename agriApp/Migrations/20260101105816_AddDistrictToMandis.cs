using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace agriApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDistrictToMandis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Mandis",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Mandis",
                keyColumn: "MandiId",
                keyValue: 1,
                column: "District",
                value: "Pune");

            migrationBuilder.UpdateData(
                table: "Mandis",
                keyColumn: "MandiId",
                keyValue: 2,
                column: "District",
                value: "Navi Mumbai");

            migrationBuilder.UpdateData(
                table: "Mandis",
                keyColumn: "MandiId",
                keyValue: 3,
                column: "District",
                value: "Nagpur");

            migrationBuilder.CreateIndex(
                name: "IX_Mandis_District",
                table: "Mandis",
                column: "District");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Mandis_District",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Mandis");
        }
    }
}
