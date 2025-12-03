using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace agriApp.Migrations
{
    /// <inheritdoc />
    public partial class AddArrivedAndAuctionLots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArrivedLots",
                columns: table => new
                {
                    ArrivedLotId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MandiId = table.Column<int>(type: "integer", nullable: false),
                    LotOwnerRole = table.Column<string>(type: "text", nullable: false),
                    LotOwnerName = table.Column<string>(type: "text", nullable: false),
                    MobileNum = table.Column<string>(type: "text", nullable: false),
                    FarmerId = table.Column<Guid>(type: "uuid", nullable: true),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: true),
                    PreLotId = table.Column<string>(type: "text", nullable: true),
                    PreRegisteredLotPreLotId = table.Column<string>(type: "character varying(10)", nullable: true),
                    MandiOfficerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CropId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<float>(type: "real", nullable: false),
                    Grade = table.Column<string>(type: "text", nullable: true),
                    LotImageUrl = table.Column<string>(type: "text", nullable: true),
                    QrCodeUrl = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArrivedLots", x => x.ArrivedLotId);
                    table.ForeignKey(
                        name: "FK_ArrivedLots_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "CropId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArrivedLots_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "FarmerId");
                    table.ForeignKey(
                        name: "FK_ArrivedLots_MandiOfficials_MandiOfficerId",
                        column: x => x.MandiOfficerId,
                        principalTable: "MandiOfficials",
                        principalColumn: "OfficialId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArrivedLots_Mandis_MandiId",
                        column: x => x.MandiId,
                        principalTable: "Mandis",
                        principalColumn: "MandiId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArrivedLots_PreRegisteredLots_PreRegisteredLotPreLotId",
                        column: x => x.PreRegisteredLotPreLotId,
                        principalTable: "PreRegisteredLots",
                        principalColumn: "PreLotId");
                    table.ForeignKey(
                        name: "FK_ArrivedLots_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "SellerId");
                });

            migrationBuilder.CreateTable(
                name: "LiveAuctionLots",
                columns: table => new
                {
                    LiveAuctionLotId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArrivedLotId = table.Column<int>(type: "integer", nullable: false),
                    AuctionStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FinalPrice = table.Column<float>(type: "real", nullable: true),
                    BuyerId = table.Column<Guid>(type: "uuid", nullable: true),
                    BuyerName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    BuyerMobile = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveAuctionLots", x => x.LiveAuctionLotId);
                    table.ForeignKey(
                        name: "FK_LiveAuctionLots_ArrivedLots_ArrivedLotId",
                        column: x => x.ArrivedLotId,
                        principalTable: "ArrivedLots",
                        principalColumn: "ArrivedLotId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LiveAuctionLots_Buyers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Buyers",
                        principalColumn: "BuyerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_UserId",
                table: "Sellers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArrivedLots_CropId",
                table: "ArrivedLots",
                column: "CropId");

            migrationBuilder.CreateIndex(
                name: "IX_ArrivedLots_FarmerId",
                table: "ArrivedLots",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_ArrivedLots_MandiId",
                table: "ArrivedLots",
                column: "MandiId");

            migrationBuilder.CreateIndex(
                name: "IX_ArrivedLots_MandiOfficerId",
                table: "ArrivedLots",
                column: "MandiOfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_ArrivedLots_PreRegisteredLotPreLotId",
                table: "ArrivedLots",
                column: "PreRegisteredLotPreLotId");

            migrationBuilder.CreateIndex(
                name: "IX_ArrivedLots_SellerId",
                table: "ArrivedLots",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAuctionLots_ArrivedLotId",
                table: "LiveAuctionLots",
                column: "ArrivedLotId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAuctionLots_AuctionStatus",
                table: "LiveAuctionLots",
                column: "AuctionStatus");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAuctionLots_BuyerId",
                table: "LiveAuctionLots",
                column: "BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sellers_UserProfiles_UserId",
                table: "Sellers",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sellers_UserProfiles_UserId",
                table: "Sellers");

            migrationBuilder.DropTable(
                name: "LiveAuctionLots");

            migrationBuilder.DropTable(
                name: "ArrivedLots");

            migrationBuilder.DropIndex(
                name: "IX_Sellers_UserId",
                table: "Sellers");
        }
    }
}
