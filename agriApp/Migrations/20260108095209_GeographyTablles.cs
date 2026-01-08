using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace agriApp.Migrations
{
    /// <inheritdoc />
    public partial class GeographyTablles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "geo_state",
                columns: table => new
                {
                    StateId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StateName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    StateCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geo_state", x => x.StateId);
                });

            migrationBuilder.CreateTable(
                name: "geo_district",
                columns: table => new
                {
                    DistrictId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DistrictName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    DistrictCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    StateId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geo_district", x => x.DistrictId);
                    table.ForeignKey(
                        name: "FK_geo_district_geo_state_StateId",
                        column: x => x.StateId,
                        principalTable: "geo_state",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "geo_taluka",
                columns: table => new
                {
                    TalukaId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TalukaName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TalukaCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    DistrictId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geo_taluka", x => x.TalukaId);
                    table.ForeignKey(
                        name: "FK_geo_taluka_geo_district_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "geo_district",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "geo_state",
                columns: new[] { "StateId", "IsActive", "StateCode", "StateName" },
                values: new object[] { 1L, true, "MH", "Maharashtra" });

            migrationBuilder.CreateIndex(
                name: "IX_geo_district_StateId_DistrictName",
                table: "geo_district",
                columns: new[] { "StateId", "DistrictName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_geo_state_StateName",
                table: "geo_state",
                column: "StateName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_geo_taluka_DistrictId_TalukaName",
                table: "geo_taluka",
                columns: new[] { "DistrictId", "TalukaName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "geo_taluka");

            migrationBuilder.DropTable(
                name: "geo_district");

            migrationBuilder.DropTable(
                name: "geo_state");
        }
    }
}
