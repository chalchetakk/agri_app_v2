using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace agriApp.Migrations
{
    /// <inheritdoc />
    public partial class lookupcategoryUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "lookup_mandi_category",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "lookup_mandi_category",
                keyColumn: "MandiCategoryId",
                keyValue: 1L,
                column: "Description",
                value: "Agricultural Produce Market Committee (regulated)");

            migrationBuilder.UpdateData(
                table: "lookup_mandi_category",
                keyColumn: "MandiCategoryId",
                keyValue: 2L,
                column: "Description",
                value: "Electronic National Agriculture Market");

            migrationBuilder.UpdateData(
                table: "lookup_mandi_category",
                keyColumn: "MandiCategoryId",
                keyValue: 3L,
                column: "Description",
                value: "Private marketplace or yard");

            migrationBuilder.InsertData(
                table: "lookup_mandi_category",
                columns: new[] { "MandiCategoryId", "CategoryName", "Description", "IsActive" },
                values: new object[,]
                {
                    { 4L, "Cooperative", "Farmer cooperative market", true },
                    { 5L, "Terminal", "Terminal market", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "lookup_mandi_category",
                keyColumn: "MandiCategoryId",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "lookup_mandi_category",
                keyColumn: "MandiCategoryId",
                keyValue: 5L);

            migrationBuilder.DropColumn(
                name: "Description",
                table: "lookup_mandi_category");
        }
    }
}
