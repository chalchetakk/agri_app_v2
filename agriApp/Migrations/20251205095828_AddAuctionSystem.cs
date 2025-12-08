using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace agriApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAuctionSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuctionId",
                table: "LiveAuctionLots",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    AuctionId = table.Column<Guid>(type: "uuid", nullable: false),
                    MandiId = table.Column<int>(type: "integer", nullable: false),
                    CropId = table.Column<int>(type: "integer", nullable: false),
                    AssignedOfficerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByOfficialId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.AuctionId);
                    table.ForeignKey(
                        name: "FK_Auctions_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "CropId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auctions_MandiOfficials_AssignedOfficerId",
                        column: x => x.AssignedOfficerId,
                        principalTable: "MandiOfficials",
                        principalColumn: "OfficialId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auctions_MandiOfficials_CreatedByOfficialId",
                        column: x => x.CreatedByOfficialId,
                        principalTable: "MandiOfficials",
                        principalColumn: "OfficialId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auctions_Mandis_MandiId",
                        column: x => x.MandiId,
                        principalTable: "Mandis",
                        principalColumn: "MandiId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiveAuctionLots_AuctionId",
                table: "LiveAuctionLots",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_AssignedOfficerId",
                table: "Auctions",
                column: "AssignedOfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_CreatedByOfficialId",
                table: "Auctions",
                column: "CreatedByOfficialId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_CropId",
                table: "Auctions",
                column: "CropId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_MandiId",
                table: "Auctions",
                column: "MandiId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_ScheduledAt",
                table: "Auctions",
                column: "ScheduledAt");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_Status",
                table: "Auctions",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveAuctionLots_Auctions_AuctionId",
                table: "LiveAuctionLots",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "AuctionId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveAuctionLots_Auctions_AuctionId",
                table: "LiveAuctionLots");

            migrationBuilder.DropTable(
                name: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_LiveAuctionLots_AuctionId",
                table: "LiveAuctionLots");

            migrationBuilder.DropColumn(
                name: "AuctionId",
                table: "LiveAuctionLots");
        }
    }
}
