using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace agriApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPreRegisteredLots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreRegisteredLots",
                columns: table => new
                {
                    PreLotId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    FarmerId = table.Column<Guid>(type: "uuid", nullable: true),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CropId = table.Column<int>(type: "integer", nullable: false),
                    MandiId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<float>(type: "real", nullable: false),
                    Grade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SellingAmount = table.Column<float>(type: "real", nullable: true),
                    LotImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    QrCodeUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExpectedArrivalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRegisteredLots", x => x.PreLotId);
                    table.ForeignKey(
                        name: "FK_PreRegisteredLots_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "CropId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreRegisteredLots_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "FarmerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreRegisteredLots_Mandis_MandiId",
                        column: x => x.MandiId,
                        principalTable: "Mandis",
                        principalColumn: "MandiId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreRegisteredLots_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "SellerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreRegisteredLots_CropId",
                table: "PreRegisteredLots",
                column: "CropId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRegisteredLots_FarmerId",
                table: "PreRegisteredLots",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRegisteredLots_MandiId",
                table: "PreRegisteredLots",
                column: "MandiId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRegisteredLots_SellerId",
                table: "PreRegisteredLots",
                column: "SellerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreRegisteredLots");
        }
    }
}
