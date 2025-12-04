using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace agriApp.Migrations
{
    /// <inheritdoc />
    public partial class FixBuyerInterestFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreRegisteredLotPreLotId",
                table: "BuyerInterestLots",
                type: "character varying(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuyerInterestLots_PreRegisteredLotPreLotId",
                table: "BuyerInterestLots",
                column: "PreRegisteredLotPreLotId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyerInterestLots_PreRegisteredLots_PreRegisteredLotPreLotId",
                table: "BuyerInterestLots",
                column: "PreRegisteredLotPreLotId",
                principalTable: "PreRegisteredLots",
                principalColumn: "PreLotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyerInterestLots_PreRegisteredLots_PreRegisteredLotPreLotId",
                table: "BuyerInterestLots");

            migrationBuilder.DropIndex(
                name: "IX_BuyerInterestLots_PreRegisteredLotPreLotId",
                table: "BuyerInterestLots");

            migrationBuilder.DropColumn(
                name: "PreRegisteredLotPreLotId",
                table: "BuyerInterestLots");
        }
    }
}
