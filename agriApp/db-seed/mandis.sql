-- ============================================
-- MANDI SEED SCRIPT
-- Depends on geo.sql + lookup_mandi_category
-- Safe to run multiple times
-- ============================================

-- 🔹 Pune Market Yard (APMC)
INSERT INTO "Mandis" (
    "MandiName",
    "Location",
    "District",
    "MandiCode",
    "MandiCategoryId",
    "StateId",
    "DistrictId",
    "TalukaId",
    "IsActive",
    "CreatedAt",
    "UpdatedAt"
)
SELECT
    'Pune Marketyard Mandi',
    'Pune',
    'Pune',
    'PMY',
    mc."MandiCategoryId",
    s."StateId",
    d."DistrictId",
    t."TalukaId",
    TRUE,
    NOW(),
    NOW()
FROM geo_state s
JOIN geo_district d ON d."StateId" = s."StateId"
LEFT JOIN geo_taluka t ON t."DistrictId" = d."DistrictId"
JOIN lookup_mandi_category mc ON mc."CategoryName" = 'APMC'
WHERE s."StateName" = 'Maharashtra'
  AND d."DistrictName" = 'Pune'
  AND t."TalukaName" = 'Haveli'
  AND NOT EXISTS (
      SELECT 1 FROM "Mandis"
      WHERE "MandiName" = 'Pune Marketyard Mandi'
  );

-- 🔹 Vashi Mandi (APMC)
INSERT INTO "Mandis" (
    "MandiName",
    "Location",
    "District",
    "MandiCode",
    "MandiCategoryId",
    "StateId",
    "DistrictId",
    "IsActive",
    "CreatedAt",
    "UpdatedAt"
)
SELECT
    'Vashi Mandi',
    'Navi Mumbai',
    'Mumbai Suburban',
    'VASHI',
    mc."MandiCategoryId",
    s."StateId",
    d."DistrictId",
    TRUE,
    NOW(),
    NOW()
FROM geo_state s
JOIN geo_district d ON d."StateId" = s."StateId"
JOIN lookup_mandi_category mc ON mc."CategoryName" = 'APMC'
WHERE s."StateName" = 'Maharashtra'
  AND d."DistrictName" = 'Mumbai Suburban'
  AND NOT EXISTS (
      SELECT 1 FROM "Mandis"
      WHERE "MandiName" = 'Vashi Mandi'
  );

-- 🔹 Nashik Mandi (APMC)
INSERT INTO "Mandis" (
    "MandiName",
    "Location",
    "District",
    "MandiCode",
    "MandiCategoryId",
    "StateId",
    "DistrictId",
    "IsActive",
    "CreatedAt",
    "UpdatedAt"
)
SELECT
    'Nashik APMC Mandi',
    'Nashik',
    'Nashik',
    'NASHIK',
    mc."MandiCategoryId",
    s."StateId",
    d."DistrictId",
    TRUE,
    NOW(),
    NOW()
FROM geo_state s
JOIN geo_district d ON d."StateId" = s."StateId"
JOIN lookup_mandi_category mc ON mc."CategoryName" = 'APMC'
WHERE s."StateName" = 'Maharashtra'
  AND d."DistrictName" = 'Nashik'
  AND NOT EXISTS (
      SELECT 1 FROM "Mandis"
      WHERE "MandiName" = 'Nashik APMC Mandi'
  );

-- 🔹 Nagpur Cotton Market (APMC)
INSERT INTO "Mandis" (
    "MandiName",
    "Location",
    "District",
    "MandiCode",
    "MandiCategoryId",
    "StateId",
    "DistrictId",
    "IsActive",
    "CreatedAt",
    "UpdatedAt"
)
SELECT
    'Nagpur Cotton Market',
    'Nagpur',
    'Nagpur',
    'NAGPUR',
    mc."MandiCategoryId",
    s."StateId",
    d."DistrictId",
    TRUE,
    NOW(),
    NOW()
FROM geo_state s
JOIN geo_district d ON d."StateId" = s."StateId"
JOIN lookup_mandi_category mc ON mc."CategoryName" = 'APMC'
WHERE s."StateName" = 'Maharashtra'
  AND d."DistrictName" = 'Nagpur'
  AND NOT EXISTS (
      SELECT 1 FROM "Mandis"
      WHERE "MandiName" = 'Nagpur Cotton Market'
  );
