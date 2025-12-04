using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace agriApp.Migrations
{
    /// <inheritdoc />
    public partial class BuyerInterestLotTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuyerInterestLots",
                columns: table => new
                {
                    BuyerInterestLotId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PreLotId = table.Column<string>(type: "character varying(10)", nullable: false),
                    BuyerId = table.Column<Guid>(type: "uuid", nullable: false),
                    BuyerBidAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyerInterestLots", x => x.BuyerInterestLotId);
                    table.ForeignKey(
                        name: "FK_BuyerInterestLots_Buyers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Buyers",
                        principalColumn: "BuyerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyerInterestLots_PreRegisteredLots_PreLotId",
                        column: x => x.PreLotId,
                        principalTable: "PreRegisteredLots",
                        principalColumn: "PreLotId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuyerInterestLots_BuyerId",
                table: "BuyerInterestLots",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyerInterestLots_PreLotId_BuyerId",
                table: "BuyerInterestLots",
                columns: new[] { "PreLotId", "BuyerId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyerInterestLots");
        }
    }
}
