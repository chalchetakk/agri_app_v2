using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace agriApp.Migrations
{
    /// <inheritdoc />
    public partial class ExtendMandisWithNullableCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Mandis",
                keyColumn: "MandiId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Mandis",
                keyColumn: "MandiId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Mandis",
                keyColumn: "MandiId",
                keyValue: 3);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "Mandis",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Mandis",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ClosingTime",
                table: "Mandis",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ColdStorageAvailable",
                table: "Mandis",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "ColdStorageCapacityMt",
                table: "Mandis",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Mandis",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "Mandis",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Mandis",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<long>(
                name: "DistrictId",
                table: "Mandis",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GradingSortingAvailable",
                table: "Mandis",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Mandis",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Mandis",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Mandis",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MandiCategoryId",
                table: "Mandis",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MandiCode",
                table: "Mandis",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OpeningTime",
                table: "Mandis",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StateId",
                table: "Mandis",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TalukaId",
                table: "Mandis",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalStorageCapacityMt",
                table: "Mandis",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Mandis",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Mandis",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeighingType",
                table: "Mandis",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkingDays",
                table: "Mandis",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mandis_DistrictId",
                table: "Mandis",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Mandis_MandiCategoryId",
                table: "Mandis",
                column: "MandiCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Mandis_MandiName",
                table: "Mandis",
                column: "MandiName");

            migrationBuilder.CreateIndex(
                name: "IX_Mandis_StateId",
                table: "Mandis",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Mandis_TalukaId",
                table: "Mandis",
                column: "TalukaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mandis_geo_district_DistrictId",
                table: "Mandis",
                column: "DistrictId",
                principalTable: "geo_district",
                principalColumn: "DistrictId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Mandis_geo_state_StateId",
                table: "Mandis",
                column: "StateId",
                principalTable: "geo_state",
                principalColumn: "StateId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Mandis_geo_taluka_TalukaId",
                table: "Mandis",
                column: "TalukaId",
                principalTable: "geo_taluka",
                principalColumn: "TalukaId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Mandis_lookup_mandi_category_MandiCategoryId",
                table: "Mandis",
                column: "MandiCategoryId",
                principalTable: "lookup_mandi_category",
                principalColumn: "MandiCategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mandis_geo_district_DistrictId",
                table: "Mandis");

            migrationBuilder.DropForeignKey(
                name: "FK_Mandis_geo_state_StateId",
                table: "Mandis");

            migrationBuilder.DropForeignKey(
                name: "FK_Mandis_geo_taluka_TalukaId",
                table: "Mandis");

            migrationBuilder.DropForeignKey(
                name: "FK_Mandis_lookup_mandi_category_MandiCategoryId",
                table: "Mandis");

            migrationBuilder.DropIndex(
                name: "IX_Mandis_DistrictId",
                table: "Mandis");

            migrationBuilder.DropIndex(
                name: "IX_Mandis_MandiCategoryId",
                table: "Mandis");

            migrationBuilder.DropIndex(
                name: "IX_Mandis_MandiName",
                table: "Mandis");

            migrationBuilder.DropIndex(
                name: "IX_Mandis_StateId",
                table: "Mandis");

            migrationBuilder.DropIndex(
                name: "IX_Mandis_TalukaId",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "ClosingTime",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "ColdStorageAvailable",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "ColdStorageCapacityMt",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "GradingSortingAvailable",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "MandiCategoryId",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "MandiCode",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "OpeningTime",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "TalukaId",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "TotalStorageCapacityMt",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "WeighingType",
                table: "Mandis");

            migrationBuilder.DropColumn(
                name: "WorkingDays",
                table: "Mandis");

            migrationBuilder.InsertData(
                table: "Mandis",
                columns: new[] { "MandiId", "District", "Location", "MandiName" },
                values: new object[,]
                {
                    { 1, "Pune", "Pune", "Pune Marketyard Mandi" },
                    { 2, "Navi Mumbai", "Mumbai", "Vashi Mandi" },
                    { 3, "Nagpur", "Nagpur", "Cotton Market" }
                });
        }
    }
}
