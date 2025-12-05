using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace agriApp.Migrations
{
    /// <inheritdoc />
    public partial class ArrivedLotConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArrivedLots_Crops_CropId",
                table: "ArrivedLots");

            migrationBuilder.DropForeignKey(
                name: "FK_ArrivedLots_Farmers_FarmerId",
                table: "ArrivedLots");

            migrationBuilder.DropForeignKey(
                name: "FK_ArrivedLots_MandiOfficials_MandiOfficerId",
                table: "ArrivedLots");

            migrationBuilder.DropForeignKey(
                name: "FK_ArrivedLots_Mandis_MandiId",
                table: "ArrivedLots");

            migrationBuilder.DropForeignKey(
                name: "FK_ArrivedLots_PreRegisteredLots_PreRegisteredLotPreLotId",
                table: "ArrivedLots");

            migrationBuilder.DropForeignKey(
                name: "FK_ArrivedLots_Sellers_SellerId",
                table: "ArrivedLots");

            migrationBuilder.DropIndex(
                name: "IX_ArrivedLots_PreRegisteredLotPreLotId",
                table: "ArrivedLots");

            migrationBuilder.DropColumn(
                name: "PreRegisteredLotPreLotId",
                table: "ArrivedLots");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ArrivedLots",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PreLotId",
                table: "ArrivedLots",
                type: "character varying(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MobileNum",
                table: "ArrivedLots",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LotOwnerRole",
                table: "ArrivedLots",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LotOwnerName",
                table: "ArrivedLots",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_ArrivedLots_MobileNum",
                table: "ArrivedLots",
                column: "MobileNum");

            migrationBuilder.CreateIndex(
                name: "IX_ArrivedLots_PreLotId",
                table: "ArrivedLots",
                column: "PreLotId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArrivedLots_Status",
                table: "ArrivedLots",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_ArrivedLots_Crops_CropId",
                table: "ArrivedLots",
                column: "CropId",
                principalTable: "Crops",
                principalColumn: "CropId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ArrivedLots_Farmers_FarmerId",
                table: "ArrivedLots",
                column: "FarmerId",
                principalTable: "Farmers",
                principalColumn: "FarmerId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ArrivedLots_MandiOfficials_MandiOfficerId",
                table: "ArrivedLots",
                column: "MandiOfficerId",
                principalTable: "MandiOfficials",
                principalColumn: "OfficialId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ArrivedLots_Mandis_MandiId",
                table: "ArrivedLots",
                column: "MandiId",
                principalTable: "Mandis",
                principalColumn: "MandiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ArrivedLots_PreRegisteredLots_PreLotId",
                table: "ArrivedLots",
                column: "PreLotId",
                principalTable: "PreRegisteredLots",
                principalColumn: "PreLotId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ArrivedLots_Sellers_SellerId",
                table: "ArrivedLots",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "SellerId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArrivedLots_Crops_CropId",
                table: "ArrivedLots");

            migrationBuilder.DropForeignKey(
                name: "FK_ArrivedLots_Farmers_FarmerId",
                table: "ArrivedLots");

            migrationBuilder.DropForeignKey(
                name: "FK_ArrivedLots_MandiOfficials_MandiOfficerId",
                table: "ArrivedLots");

            migrationBuilder.DropForeignKey(
                name: "FK_ArrivedLots_Mandis_MandiId",
                table: "ArrivedLots");

            migrationBuilder.DropForeignKey(
                name: "FK_ArrivedLots_PreRegisteredLots_PreLotId",
                table: "ArrivedLots");

            migrationBuilder.DropForeignKey(
                name: "FK_ArrivedLots_Sellers_SellerId",
                table: "ArrivedLots");

            migrationBuilder.DropIndex(
                name: "IX_ArrivedLots_MobileNum",
                table: "ArrivedLots");

            migrationBuilder.DropIndex(
                name: "IX_ArrivedLots_PreLotId",
                table: "ArrivedLots");

            migrationBuilder.DropIndex(
                name: "IX_ArrivedLots_Status",
                table: "ArrivedLots");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ArrivedLots",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PreLotId",
                table: "ArrivedLots",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MobileNum",
                table: "ArrivedLots",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "LotOwnerRole",
                table: "ArrivedLots",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "LotOwnerName",
                table: "ArrivedLots",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "PreRegisteredLotPreLotId",
                table: "ArrivedLots",
                type: "character varying(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArrivedLots_PreRegisteredLotPreLotId",
                table: "ArrivedLots",
                column: "PreRegisteredLotPreLotId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArrivedLots_Crops_CropId",
                table: "ArrivedLots",
                column: "CropId",
                principalTable: "Crops",
                principalColumn: "CropId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArrivedLots_Farmers_FarmerId",
                table: "ArrivedLots",
                column: "FarmerId",
                principalTable: "Farmers",
                principalColumn: "FarmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArrivedLots_MandiOfficials_MandiOfficerId",
                table: "ArrivedLots",
                column: "MandiOfficerId",
                principalTable: "MandiOfficials",
                principalColumn: "OfficialId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArrivedLots_Mandis_MandiId",
                table: "ArrivedLots",
                column: "MandiId",
                principalTable: "Mandis",
                principalColumn: "MandiId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArrivedLots_PreRegisteredLots_PreRegisteredLotPreLotId",
                table: "ArrivedLots",
                column: "PreRegisteredLotPreLotId",
                principalTable: "PreRegisteredLots",
                principalColumn: "PreLotId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArrivedLots_Sellers_SellerId",
                table: "ArrivedLots",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "SellerId");
        }
    }
}
