CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "UserProfiles" (
    "UserProfileId" uuid NOT NULL,
    "MobileNumber" character varying(10) NOT NULL,
    "PreferredLanguage" character varying(10) NOT NULL,
    "IsVerified" boolean NOT NULL,
    "LastLoginAt" timestamp with time zone,
    "CreationTime" timestamp with time zone NOT NULL,
    "LastModificationTime" timestamp with time zone,
    CONSTRAINT "PK_UserProfiles" PRIMARY KEY ("UserProfileId")
);

CREATE TABLE "JwtTokens" (
    "JwtTokenId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "AccessTokenJti" text NOT NULL,
    "AccessTokenIssuedAt" timestamp with time zone NOT NULL,
    "AccessTokenExpiresAt" timestamp with time zone NOT NULL,
    "RefreshTokenHash" text NOT NULL,
    "RefreshTokenIssuedAt" timestamp with time zone NOT NULL,
    "RefreshTokenExpiresAt" timestamp with time zone NOT NULL,
    "IsRevoked" boolean NOT NULL,
    "RevokedAt" timestamp with time zone,
    "ReplacedByTokenId" uuid,
    "DeviceInfo" character varying(200) NOT NULL,
    "IpAddress" character varying(50) NOT NULL,
    CONSTRAINT "PK_JwtTokens" PRIMARY KEY ("JwtTokenId"),
    CONSTRAINT "FK_JwtTokens_JwtTokens_ReplacedByTokenId" FOREIGN KEY ("ReplacedByTokenId") REFERENCES "JwtTokens" ("JwtTokenId") ON DELETE RESTRICT,
    CONSTRAINT "FK_JwtTokens_UserProfiles_UserId" FOREIGN KEY ("UserId") REFERENCES "UserProfiles" ("UserProfileId") ON DELETE CASCADE
);

CREATE TABLE "LoginActivities" (
    "ActivityId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "LoginTime" timestamp with time zone NOT NULL,
    "IsSuccessful" boolean NOT NULL,
    "FailureReason" character varying(500),
    "IpAddress" character varying(50),
    "DeviceInfo" character varying(200),
    CONSTRAINT "PK_LoginActivities" PRIMARY KEY ("ActivityId"),
    CONSTRAINT "FK_LoginActivities_UserProfiles_UserId" FOREIGN KEY ("UserId") REFERENCES "UserProfiles" ("UserProfileId") ON DELETE CASCADE
);

CREATE TABLE "Otps" (
    "OtpId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "OtpHash" text NOT NULL,
    "ExpireAt" timestamp with time zone NOT NULL,
    "IsUsed" boolean NOT NULL,
    "UsedAt" timestamp with time zone,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Otps" PRIMARY KEY ("OtpId"),
    CONSTRAINT "FK_Otps_UserProfiles_UserId" FOREIGN KEY ("UserId") REFERENCES "UserProfiles" ("UserProfileId") ON DELETE CASCADE
);

CREATE INDEX "IX_JwtTokens_AccessTokenJti" ON "JwtTokens" ("AccessTokenJti");

CREATE INDEX "IX_JwtTokens_RefreshTokenHash" ON "JwtTokens" ("RefreshTokenHash");

CREATE INDEX "IX_JwtTokens_ReplacedByTokenId" ON "JwtTokens" ("ReplacedByTokenId");

CREATE INDEX "IX_JwtTokens_UserId" ON "JwtTokens" ("UserId");

CREATE INDEX "IX_LoginActivities_UserId" ON "LoginActivities" ("UserId");

CREATE INDEX "IX_Otps_UserId" ON "Otps" ("UserId");

CREATE UNIQUE INDEX "IX_UserProfiles_MobileNumber" ON "UserProfiles" ("MobileNumber");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251120130344_InitialCreate', '8.0.5');

COMMIT;

START TRANSACTION;

CREATE TABLE "Farmers" (
    "FarmerId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "FarmerName" character varying(200) NOT NULL,
    "Location" character varying(200),
    "ProfilePhotoUrl" character varying(500),
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Farmers" PRIMARY KEY ("FarmerId"),
    CONSTRAINT "FK_Farmers_UserProfiles_UserId" FOREIGN KEY ("UserId") REFERENCES "UserProfiles" ("UserProfileId") ON DELETE CASCADE
);

CREATE TABLE "FarmDetails" (
    "FarmId" uuid NOT NULL,
    "FarmerId" uuid NOT NULL,
    "FarmLocation" character varying(200) NOT NULL,
    "PrimaryCrop" character varying(100) NOT NULL,
    "FarmSize" real NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_FarmDetails" PRIMARY KEY ("FarmId"),
    CONSTRAINT "FK_FarmDetails_Farmers_FarmerId" FOREIGN KEY ("FarmerId") REFERENCES "Farmers" ("FarmerId") ON DELETE CASCADE
);

CREATE TABLE "FarmerInterestedCrops" (
    "FarmerInterestedCropId" uuid NOT NULL,
    "FarmerId" uuid NOT NULL,
    "CropId" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_FarmerInterestedCrops" PRIMARY KEY ("FarmerInterestedCropId"),
    CONSTRAINT "FK_FarmerInterestedCrops_Farmers_FarmerId" FOREIGN KEY ("FarmerId") REFERENCES "Farmers" ("FarmerId") ON DELETE CASCADE
);

CREATE INDEX "IX_FarmDetails_FarmerId" ON "FarmDetails" ("FarmerId");

CREATE UNIQUE INDEX "IX_FarmerInterestedCrops_FarmerId_CropId" ON "FarmerInterestedCrops" ("FarmerId", "CropId");

CREATE INDEX "IX_Farmers_UserId" ON "Farmers" ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251125092530_FarmerModule', '8.0.5');

COMMIT;

START TRANSACTION;

CREATE TABLE "Crops" (
    "CropId" integer GENERATED BY DEFAULT AS IDENTITY,
    "CropName" character varying(100) NOT NULL,
    "Grade" character varying(50),
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Crops" PRIMARY KEY ("CropId")
);

CREATE UNIQUE INDEX "IX_Crops_CropName" ON "Crops" ("CropName");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251125104651_AddCropsTable', '8.0.5');

COMMIT;

START TRANSACTION;

CREATE TABLE "Buyers" (
    "BuyerId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "BuyerName" character varying(200) NOT NULL,
    "BusinessId" character varying(50),
    "BusinessName" character varying(200) NOT NULL,
    "Location" character varying(200),
    "ProfilePhotoUrl" character varying(500),
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Buyers" PRIMARY KEY ("BuyerId"),
    CONSTRAINT "FK_Buyers_UserProfiles_UserId" FOREIGN KEY ("UserId") REFERENCES "UserProfiles" ("UserProfileId") ON DELETE CASCADE
);

CREATE TABLE "BuyerInterestedCrops" (
    "BuyerInterestedCropId" uuid NOT NULL,
    "BuyerId" uuid NOT NULL,
    "CropId" integer NOT NULL,
    CONSTRAINT "PK_BuyerInterestedCrops" PRIMARY KEY ("BuyerInterestedCropId"),
    CONSTRAINT "FK_BuyerInterestedCrops_Buyers_BuyerId" FOREIGN KEY ("BuyerId") REFERENCES "Buyers" ("BuyerId") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_BuyerInterestedCrops_BuyerId_CropId" ON "BuyerInterestedCrops" ("BuyerId", "CropId");

CREATE INDEX "IX_Buyers_UserId" ON "Buyers" ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251125114606_AddBuyerEntities', '8.0.5');

COMMIT;

START TRANSACTION;

CREATE TABLE "Sellers" (
    "SellerId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "SellerName" text NOT NULL,
    "BusinessName" text,
    "Location" text NOT NULL,
    "ProfilePhotoUrl" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Sellers" PRIMARY KEY ("SellerId")
);

CREATE TABLE "SellerInterestedCrops" (
    "SellerInterestedCropId" integer GENERATED BY DEFAULT AS IDENTITY,
    "SellerId" uuid NOT NULL,
    "CropId" integer NOT NULL,
    CONSTRAINT "PK_SellerInterestedCrops" PRIMARY KEY ("SellerInterestedCropId"),
    CONSTRAINT "FK_SellerInterestedCrops_Crops_CropId" FOREIGN KEY ("CropId") REFERENCES "Crops" ("CropId") ON DELETE CASCADE,
    CONSTRAINT "FK_SellerInterestedCrops_Sellers_SellerId" FOREIGN KEY ("SellerId") REFERENCES "Sellers" ("SellerId") ON DELETE CASCADE
);

CREATE INDEX "IX_SellerInterestedCrops_CropId" ON "SellerInterestedCrops" ("CropId");

CREATE UNIQUE INDEX "IX_SellerInterestedCrops_SellerId_CropId" ON "SellerInterestedCrops" ("SellerId", "CropId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251125130415_AddSellerTables', '8.0.5');

COMMIT;

START TRANSACTION;

CREATE TABLE "Mandis" (
    "MandiId" integer GENERATED BY DEFAULT AS IDENTITY,
    "MandiName" character varying(200) NOT NULL,
    "Location" character varying(200) NOT NULL,
    CONSTRAINT "PK_Mandis" PRIMARY KEY ("MandiId")
);

CREATE TABLE "OfficialRoles" (
    "OfficialRoleId" uuid NOT NULL,
    "OfficialRoleName" character varying(100) NOT NULL,
    "RoleCode" text NOT NULL,
    CONSTRAINT "PK_OfficialRoles" PRIMARY KEY ("OfficialRoleId")
);

CREATE TABLE "MandiOfficials" (
    "OfficialId" uuid NOT NULL,
    "OfficialName" character varying(200) NOT NULL,
    "EmployeeId" character varying(100) NOT NULL,
    "Email" character varying(200) NOT NULL,
    "MandiId" integer NOT NULL,
    "OfficialRoleId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_MandiOfficials" PRIMARY KEY ("OfficialId"),
    CONSTRAINT "FK_MandiOfficials_Mandis_MandiId" FOREIGN KEY ("MandiId") REFERENCES "Mandis" ("MandiId") ON DELETE RESTRICT,
    CONSTRAINT "FK_MandiOfficials_OfficialRoles_OfficialRoleId" FOREIGN KEY ("OfficialRoleId") REFERENCES "OfficialRoles" ("OfficialRoleId") ON DELETE RESTRICT,
    CONSTRAINT "FK_MandiOfficials_UserProfiles_UserId" FOREIGN KEY ("UserId") REFERENCES "UserProfiles" ("UserProfileId") ON DELETE CASCADE
);

CREATE INDEX "IX_MandiOfficials_MandiId" ON "MandiOfficials" ("MandiId");

CREATE INDEX "IX_MandiOfficials_OfficialRoleId" ON "MandiOfficials" ("OfficialRoleId");

CREATE INDEX "IX_MandiOfficials_UserId" ON "MandiOfficials" ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251127063833_AddMandiOfficialModule', '8.0.5');

COMMIT;

START TRANSACTION;

INSERT INTO "Mandis" ("MandiId", "Location", "MandiName")
VALUES (1, 'Pune', 'Pune Marketyard Mandi');
INSERT INTO "Mandis" ("MandiId", "Location", "MandiName")
VALUES (2, 'Mumbai', 'Vashi Mandi');
INSERT INTO "Mandis" ("MandiId", "Location", "MandiName")
VALUES (3, 'Nagpur', 'Cotton Market');

INSERT INTO "OfficialRoles" ("OfficialRoleId", "OfficialRoleName", "RoleCode")
VALUES ('11111111-1111-1111-1111-111111111111', 'Mandi Officer', 'OFFICER');
INSERT INTO "OfficialRoles" ("OfficialRoleId", "OfficialRoleName", "RoleCode")
VALUES ('22222222-2222-2222-2222-222222222222', 'Mandi Manager', 'MANAGER');
INSERT INTO "OfficialRoles" ("OfficialRoleId", "OfficialRoleName", "RoleCode")
VALUES ('33333333-3333-3333-3333-333333333333', 'Mandi Approver', 'APPROVER');

SELECT setval(
    pg_get_serial_sequence('"Mandis"', 'MandiId'),
    GREATEST(
        (SELECT MAX("MandiId") FROM "Mandis") + 1,
        nextval(pg_get_serial_sequence('"Mandis"', 'MandiId'))),
    false);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251127092646_SeedMandisAndRoles', '8.0.5');

COMMIT;

START TRANSACTION;

CREATE TABLE "PreRegisteredLots" (
    "PreLotId" character varying(10) NOT NULL,
    "FarmerId" uuid,
    "SellerId" uuid,
    "CropId" integer NOT NULL,
    "MandiId" integer NOT NULL,
    "Status" character varying(50) NOT NULL,
    "Quantity" real NOT NULL,
    "Grade" character varying(100) NOT NULL,
    "SellingAmount" real,
    "LotImageUrl" character varying(500) NOT NULL,
    "QrCodeUrl" character varying(500) NOT NULL,
    "ExpectedArrivalDate" timestamp with time zone NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_PreRegisteredLots" PRIMARY KEY ("PreLotId"),
    CONSTRAINT "FK_PreRegisteredLots_Crops_CropId" FOREIGN KEY ("CropId") REFERENCES "Crops" ("CropId") ON DELETE RESTRICT,
    CONSTRAINT "FK_PreRegisteredLots_Farmers_FarmerId" FOREIGN KEY ("FarmerId") REFERENCES "Farmers" ("FarmerId") ON DELETE RESTRICT,
    CONSTRAINT "FK_PreRegisteredLots_Mandis_MandiId" FOREIGN KEY ("MandiId") REFERENCES "Mandis" ("MandiId") ON DELETE RESTRICT,
    CONSTRAINT "FK_PreRegisteredLots_Sellers_SellerId" FOREIGN KEY ("SellerId") REFERENCES "Sellers" ("SellerId") ON DELETE RESTRICT
);

CREATE INDEX "IX_PreRegisteredLots_CropId" ON "PreRegisteredLots" ("CropId");

CREATE INDEX "IX_PreRegisteredLots_FarmerId" ON "PreRegisteredLots" ("FarmerId");

CREATE INDEX "IX_PreRegisteredLots_MandiId" ON "PreRegisteredLots" ("MandiId");

CREATE INDEX "IX_PreRegisteredLots_SellerId" ON "PreRegisteredLots" ("SellerId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251127124810_AddPreRegisteredLots', '8.0.5');

COMMIT;

START TRANSACTION;

ALTER TABLE "PreRegisteredLots" ALTER COLUMN "QrCodeUrl" DROP NOT NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251128045804_MakeQrCodeNullable', '8.0.5');

COMMIT;

START TRANSACTION;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251128045911_MakeQrNullable', '8.0.5');

COMMIT;

START TRANSACTION;

CREATE TABLE "ArrivedLots" (
    "ArrivedLotId" integer GENERATED BY DEFAULT AS IDENTITY,
    "MandiId" integer NOT NULL,
    "LotOwnerRole" text NOT NULL,
    "LotOwnerName" text NOT NULL,
    "MobileNum" text NOT NULL,
    "FarmerId" uuid,
    "SellerId" uuid,
    "PreLotId" text,
    "PreRegisteredLotPreLotId" character varying(10),
    "MandiOfficerId" uuid NOT NULL,
    "CropId" integer NOT NULL,
    "Quantity" real NOT NULL,
    "Grade" text,
    "LotImageUrl" text,
    "QrCodeUrl" text NOT NULL,
    "Status" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_ArrivedLots" PRIMARY KEY ("ArrivedLotId"),
    CONSTRAINT "FK_ArrivedLots_Crops_CropId" FOREIGN KEY ("CropId") REFERENCES "Crops" ("CropId") ON DELETE CASCADE,
    CONSTRAINT "FK_ArrivedLots_Farmers_FarmerId" FOREIGN KEY ("FarmerId") REFERENCES "Farmers" ("FarmerId"),
    CONSTRAINT "FK_ArrivedLots_MandiOfficials_MandiOfficerId" FOREIGN KEY ("MandiOfficerId") REFERENCES "MandiOfficials" ("OfficialId") ON DELETE CASCADE,
    CONSTRAINT "FK_ArrivedLots_Mandis_MandiId" FOREIGN KEY ("MandiId") REFERENCES "Mandis" ("MandiId") ON DELETE CASCADE,
    CONSTRAINT "FK_ArrivedLots_PreRegisteredLots_PreRegisteredLotPreLotId" FOREIGN KEY ("PreRegisteredLotPreLotId") REFERENCES "PreRegisteredLots" ("PreLotId"),
    CONSTRAINT "FK_ArrivedLots_Sellers_SellerId" FOREIGN KEY ("SellerId") REFERENCES "Sellers" ("SellerId")
);

CREATE TABLE "LiveAuctionLots" (
    "LiveAuctionLotId" integer GENERATED BY DEFAULT AS IDENTITY,
    "ArrivedLotId" integer NOT NULL,
    "AuctionStatus" character varying(20) NOT NULL,
    "FinalPrice" real,
    "BuyerId" uuid,
    "BuyerName" character varying(150),
    "BuyerMobile" character varying(10),
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_LiveAuctionLots" PRIMARY KEY ("LiveAuctionLotId"),
    CONSTRAINT "FK_LiveAuctionLots_ArrivedLots_ArrivedLotId" FOREIGN KEY ("ArrivedLotId") REFERENCES "ArrivedLots" ("ArrivedLotId") ON DELETE CASCADE,
    CONSTRAINT "FK_LiveAuctionLots_Buyers_BuyerId" FOREIGN KEY ("BuyerId") REFERENCES "Buyers" ("BuyerId") ON DELETE RESTRICT
);

CREATE INDEX "IX_Sellers_UserId" ON "Sellers" ("UserId");

CREATE INDEX "IX_ArrivedLots_CropId" ON "ArrivedLots" ("CropId");

CREATE INDEX "IX_ArrivedLots_FarmerId" ON "ArrivedLots" ("FarmerId");

CREATE INDEX "IX_ArrivedLots_MandiId" ON "ArrivedLots" ("MandiId");

CREATE INDEX "IX_ArrivedLots_MandiOfficerId" ON "ArrivedLots" ("MandiOfficerId");

CREATE INDEX "IX_ArrivedLots_PreRegisteredLotPreLotId" ON "ArrivedLots" ("PreRegisteredLotPreLotId");

CREATE INDEX "IX_ArrivedLots_SellerId" ON "ArrivedLots" ("SellerId");

CREATE INDEX "IX_LiveAuctionLots_ArrivedLotId" ON "LiveAuctionLots" ("ArrivedLotId");

CREATE INDEX "IX_LiveAuctionLots_AuctionStatus" ON "LiveAuctionLots" ("AuctionStatus");

CREATE INDEX "IX_LiveAuctionLots_BuyerId" ON "LiveAuctionLots" ("BuyerId");

ALTER TABLE "Sellers" ADD CONSTRAINT "FK_Sellers_UserProfiles_UserId" FOREIGN KEY ("UserId") REFERENCES "UserProfiles" ("UserProfileId") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251202124715_AddArrivedAndAuctionLots', '8.0.5');

COMMIT;

START TRANSACTION;

CREATE TABLE "BuyerInterestLots" (
    "BuyerInterestLotId" integer GENERATED BY DEFAULT AS IDENTITY,
    "PreLotId" character varying(10) NOT NULL,
    "BuyerId" uuid NOT NULL,
    "BuyerBidAmount" numeric(10,2) NOT NULL,
    "Status" character varying(20) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_BuyerInterestLots" PRIMARY KEY ("BuyerInterestLotId"),
    CONSTRAINT "FK_BuyerInterestLots_Buyers_BuyerId" FOREIGN KEY ("BuyerId") REFERENCES "Buyers" ("BuyerId") ON DELETE CASCADE,
    CONSTRAINT "FK_BuyerInterestLots_PreRegisteredLots_PreLotId" FOREIGN KEY ("PreLotId") REFERENCES "PreRegisteredLots" ("PreLotId") ON DELETE CASCADE
);

CREATE INDEX "IX_BuyerInterestLots_BuyerId" ON "BuyerInterestLots" ("BuyerId");

CREATE UNIQUE INDEX "IX_BuyerInterestLots_PreLotId_BuyerId" ON "BuyerInterestLots" ("PreLotId", "BuyerId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251204073859_BuyerInterestLotTable', '8.0.5');

COMMIT;

START TRANSACTION;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251204074539_BuyerInterestLot_Table', '8.0.5');

COMMIT;

START TRANSACTION;

ALTER TABLE "BuyerInterestLots" ADD "PreRegisteredLotPreLotId" character varying(10);

CREATE INDEX "IX_BuyerInterestLots_PreRegisteredLotPreLotId" ON "BuyerInterestLots" ("PreRegisteredLotPreLotId");

ALTER TABLE "BuyerInterestLots" ADD CONSTRAINT "FK_BuyerInterestLots_PreRegisteredLots_PreRegisteredLotPreLotId" FOREIGN KEY ("PreRegisteredLotPreLotId") REFERENCES "PreRegisteredLots" ("PreLotId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251204093448_FixBuyerInterestFK', '8.0.5');

COMMIT;

START TRANSACTION;

ALTER TABLE "Buyers" ADD "Email" character varying(100);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251204102302_AddEmailToBuyer', '8.0.5');

COMMIT;

START TRANSACTION;

ALTER TABLE "ArrivedLots" DROP CONSTRAINT "FK_ArrivedLots_Crops_CropId";

ALTER TABLE "ArrivedLots" DROP CONSTRAINT "FK_ArrivedLots_Farmers_FarmerId";

ALTER TABLE "ArrivedLots" DROP CONSTRAINT "FK_ArrivedLots_MandiOfficials_MandiOfficerId";

ALTER TABLE "ArrivedLots" DROP CONSTRAINT "FK_ArrivedLots_Mandis_MandiId";

ALTER TABLE "ArrivedLots" DROP CONSTRAINT "FK_ArrivedLots_PreRegisteredLots_PreRegisteredLotPreLotId";

ALTER TABLE "ArrivedLots" DROP CONSTRAINT "FK_ArrivedLots_Sellers_SellerId";

DROP INDEX "IX_ArrivedLots_PreRegisteredLotPreLotId";

ALTER TABLE "ArrivedLots" DROP COLUMN "PreRegisteredLotPreLotId";

ALTER TABLE "ArrivedLots" ALTER COLUMN "Status" TYPE character varying(50);

ALTER TABLE "ArrivedLots" ALTER COLUMN "PreLotId" TYPE character varying(10);

ALTER TABLE "ArrivedLots" ALTER COLUMN "MobileNum" TYPE character varying(10);

ALTER TABLE "ArrivedLots" ALTER COLUMN "LotOwnerRole" TYPE character varying(20);

ALTER TABLE "ArrivedLots" ALTER COLUMN "LotOwnerName" TYPE character varying(100);

CREATE INDEX "IX_ArrivedLots_MobileNum" ON "ArrivedLots" ("MobileNum");

CREATE UNIQUE INDEX "IX_ArrivedLots_PreLotId" ON "ArrivedLots" ("PreLotId");

CREATE INDEX "IX_ArrivedLots_Status" ON "ArrivedLots" ("Status");

ALTER TABLE "ArrivedLots" ADD CONSTRAINT "FK_ArrivedLots_Crops_CropId" FOREIGN KEY ("CropId") REFERENCES "Crops" ("CropId") ON DELETE RESTRICT;

ALTER TABLE "ArrivedLots" ADD CONSTRAINT "FK_ArrivedLots_Farmers_FarmerId" FOREIGN KEY ("FarmerId") REFERENCES "Farmers" ("FarmerId") ON DELETE SET NULL;

ALTER TABLE "ArrivedLots" ADD CONSTRAINT "FK_ArrivedLots_MandiOfficials_MandiOfficerId" FOREIGN KEY ("MandiOfficerId") REFERENCES "MandiOfficials" ("OfficialId") ON DELETE RESTRICT;

ALTER TABLE "ArrivedLots" ADD CONSTRAINT "FK_ArrivedLots_Mandis_MandiId" FOREIGN KEY ("MandiId") REFERENCES "Mandis" ("MandiId") ON DELETE RESTRICT;

ALTER TABLE "ArrivedLots" ADD CONSTRAINT "FK_ArrivedLots_PreRegisteredLots_PreLotId" FOREIGN KEY ("PreLotId") REFERENCES "PreRegisteredLots" ("PreLotId") ON DELETE SET NULL;

ALTER TABLE "ArrivedLots" ADD CONSTRAINT "FK_ArrivedLots_Sellers_SellerId" FOREIGN KEY ("SellerId") REFERENCES "Sellers" ("SellerId") ON DELETE SET NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251205070501_ArrivedLotConfig', '8.0.5');

COMMIT;

START TRANSACTION;

ALTER TABLE "LiveAuctionLots" ADD "AuctionId" uuid;

CREATE TABLE "Auctions" (
    "AuctionId" uuid NOT NULL,
    "MandiId" integer NOT NULL,
    "CropId" integer NOT NULL,
    "AssignedOfficerId" uuid NOT NULL,
    "CreatedByOfficialId" uuid NOT NULL,
    "ScheduledAt" timestamp with time zone NOT NULL,
    "Status" character varying(20) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Auctions" PRIMARY KEY ("AuctionId"),
    CONSTRAINT "FK_Auctions_Crops_CropId" FOREIGN KEY ("CropId") REFERENCES "Crops" ("CropId") ON DELETE RESTRICT,
    CONSTRAINT "FK_Auctions_MandiOfficials_AssignedOfficerId" FOREIGN KEY ("AssignedOfficerId") REFERENCES "MandiOfficials" ("OfficialId") ON DELETE RESTRICT,
    CONSTRAINT "FK_Auctions_MandiOfficials_CreatedByOfficialId" FOREIGN KEY ("CreatedByOfficialId") REFERENCES "MandiOfficials" ("OfficialId") ON DELETE RESTRICT,
    CONSTRAINT "FK_Auctions_Mandis_MandiId" FOREIGN KEY ("MandiId") REFERENCES "Mandis" ("MandiId") ON DELETE RESTRICT
);

CREATE INDEX "IX_LiveAuctionLots_AuctionId" ON "LiveAuctionLots" ("AuctionId");

CREATE INDEX "IX_Auctions_AssignedOfficerId" ON "Auctions" ("AssignedOfficerId");

CREATE INDEX "IX_Auctions_CreatedByOfficialId" ON "Auctions" ("CreatedByOfficialId");

CREATE INDEX "IX_Auctions_CropId" ON "Auctions" ("CropId");

CREATE INDEX "IX_Auctions_MandiId" ON "Auctions" ("MandiId");

CREATE INDEX "IX_Auctions_ScheduledAt" ON "Auctions" ("ScheduledAt");

CREATE INDEX "IX_Auctions_Status" ON "Auctions" ("Status");

ALTER TABLE "LiveAuctionLots" ADD CONSTRAINT "FK_LiveAuctionLots_Auctions_AuctionId" FOREIGN KEY ("AuctionId") REFERENCES "Auctions" ("AuctionId") ON DELETE SET NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251205095828_AddAuctionSystem', '8.0.5');

COMMIT;

START TRANSACTION;

CREATE TABLE "Anchors" (
    "AnchorId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "CompanyName" character varying(200) NOT NULL,
    "RegistrationNumber" character varying(100) NOT NULL,
    "CompanyAddress" character varying(500) NOT NULL,
    "ContactPersonName" character varying(200) NOT NULL,
    "Email" character varying(150) NOT NULL,
    "ContactPersonNum" character varying(10) NOT NULL,
    "GSTNumber" character varying(20),
    "EstimatedFarmersNum" integer NOT NULL,
    "BusinessDescription" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Anchors" PRIMARY KEY ("AnchorId"),
    CONSTRAINT "FK_Anchors_UserProfiles_UserId" FOREIGN KEY ("UserId") REFERENCES "UserProfiles" ("UserProfileId") ON DELETE CASCADE
);

CREATE INDEX "IX_Anchors_UserId" ON "Anchors" ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251209123510_AddAnchorTable', '8.0.5');

COMMIT;

START TRANSACTION;

CREATE TABLE "AnchorFarmers" (
    "AnchorFarmerId" uuid NOT NULL,
    "AnchorId" uuid NOT NULL,
    "FarmerId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_AnchorFarmers" PRIMARY KEY ("AnchorFarmerId"),
    CONSTRAINT "FK_AnchorFarmers_Anchors_AnchorId" FOREIGN KEY ("AnchorId") REFERENCES "Anchors" ("AnchorId") ON DELETE CASCADE,
    CONSTRAINT "FK_AnchorFarmers_Farmers_FarmerId" FOREIGN KEY ("FarmerId") REFERENCES "Farmers" ("FarmerId") ON DELETE CASCADE
);

CREATE INDEX "IX_AnchorFarmers_AnchorId" ON "AnchorFarmers" ("AnchorId");

CREATE UNIQUE INDEX "IX_AnchorFarmers_FarmerId" ON "AnchorFarmers" ("FarmerId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251210121014_AddAnchorFarmersTable', '8.0.5');

COMMIT;

START TRANSACTION;

ALTER TABLE "Mandis" ADD "District" character varying(150) NOT NULL DEFAULT '';

UPDATE "Mandis" SET "District" = 'Pune'
WHERE "MandiId" = 1;

UPDATE "Mandis" SET "District" = 'Navi Mumbai'
WHERE "MandiId" = 2;

UPDATE "Mandis" SET "District" = 'Nagpur'
WHERE "MandiId" = 3;

CREATE INDEX "IX_Mandis_District" ON "Mandis" ("District");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260101105816_AddDistrictToMandis', '8.0.5');

COMMIT;

START TRANSACTION;

ALTER TABLE "UserProfiles" ADD "UserName" character varying(50);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260102080305_AddUserNameToUserProfile', '8.0.5');

COMMIT;

