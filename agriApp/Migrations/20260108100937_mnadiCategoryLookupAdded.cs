using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace agriApp.Migrations
{
    /// <inheritdoc />
    public partial class mnadiCategoryLookupAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lookup_mandi_category",
                columns: table => new
                {
                    MandiCategoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lookup_mandi_category", x => x.MandiCategoryId);
                });

            migrationBuilder.InsertData(
                table: "lookup_mandi_category",
                columns: new[] { "MandiCategoryId", "CategoryName", "IsActive" },
                values: new object[,]
                {
                    { 1L, "APMC", true },
                    { 2L, "eNAM", true },
                    { 3L, "Private", true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_lookup_mandi_category_CategoryName",
                table: "lookup_mandi_category",
                column: "CategoryName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lookup_mandi_category");
        }
    }
}
