using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace agriApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    UserProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    MobileNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PreferredLanguage = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.UserProfileId);
                });

            migrationBuilder.CreateTable(
                name: "JwtTokens",
                columns: table => new
                {
                    JwtTokenId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccessTokenJti = table.Column<string>(type: "text", nullable: false),
                    AccessTokenIssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccessTokenExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RefreshTokenHash = table.Column<string>(type: "text", nullable: false),
                    RefreshTokenIssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RefreshTokenExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReplacedByTokenId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeviceInfo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwtTokens", x => x.JwtTokenId);
                    table.ForeignKey(
                        name: "FK_JwtTokens_JwtTokens_ReplacedByTokenId",
                        column: x => x.ReplacedByTokenId,
                        principalTable: "JwtTokens",
                        principalColumn: "JwtTokenId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JwtTokens_UserProfiles_UserId",
                        column: x => x.UserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoginActivities",
                columns: table => new
                {
                    ActivityId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "boolean", nullable: false),
                    FailureReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DeviceInfo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginActivities", x => x.ActivityId);
                    table.ForeignKey(
                        name: "FK_LoginActivities_UserProfiles_UserId",
                        column: x => x.UserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Otps",
                columns: table => new
                {
                    OtpId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OtpHash = table.Column<string>(type: "text", nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Otps", x => x.OtpId);
                    table.ForeignKey(
                        name: "FK_Otps_UserProfiles_UserId",
                        column: x => x.UserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JwtTokens_AccessTokenJti",
                table: "JwtTokens",
                column: "AccessTokenJti");

            migrationBuilder.CreateIndex(
                name: "IX_JwtTokens_RefreshTokenHash",
                table: "JwtTokens",
                column: "RefreshTokenHash");

            migrationBuilder.CreateIndex(
                name: "IX_JwtTokens_ReplacedByTokenId",
                table: "JwtTokens",
                column: "ReplacedByTokenId");

            migrationBuilder.CreateIndex(
                name: "IX_JwtTokens_UserId",
                table: "JwtTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginActivities_UserId",
                table: "LoginActivities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Otps_UserId",
                table: "Otps",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_MobileNumber",
                table: "UserProfiles",
                column: "MobileNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JwtTokens");

            migrationBuilder.DropTable(
                name: "LoginActivities");

            migrationBuilder.DropTable(
                name: "Otps");

            migrationBuilder.DropTable(
                name: "UserProfiles");
        }
    }
}
