-- ============================================
-- GEO SEED SCRIPT
-- Safe to run multiple times
-- ============================================

-- 1️⃣ Ensure Maharashtra state exists
INSERT INTO geo_state ("StateName", "IsActive")
SELECT 'Maharashtra', TRUE
WHERE NOT EXISTS (
    SELECT 1 FROM geo_state WHERE "StateName" = 'Maharashtra'
);

-- 2️⃣ Ensure districts for Maharashtra exist
INSERT INTO geo_district ("DistrictName", "StateId", "IsActive")
SELECT d.district_name, s."StateId", TRUE
FROM (
    VALUES
        ('Pune'),
        ('Mumbai Suburban'),
        ('Mumbai City'),
        ('Nashik'),
        ('Nagpur')
) AS d(district_name)
JOIN geo_state s ON s."StateName" = 'Maharashtra'
WHERE NOT EXISTS (
    SELECT 1
    FROM geo_district gd
    WHERE gd."DistrictName" = d.district_name
      AND gd."StateId" = s."StateId"
);

-- 3️⃣ Talukas (OPTIONAL – you can add later)
-- Example for Pune district
INSERT INTO geo_taluka ("TalukaName", "DistrictId", "IsActive")
SELECT t.taluka_name, d."DistrictId", TRUE
FROM (
    VALUES
        ('Haveli'),
        ('Junnar'),
        ('Mulshi')
) AS t(taluka_name)
JOIN geo_district d ON d."DistrictName" = 'Pune'
JOIN geo_state s ON s."StateId" = d."StateId"
WHERE s."StateName" = 'Maharashtra'
  AND NOT EXISTS (
      SELECT 1
      FROM geo_taluka gt
      WHERE gt."TalukaName" = t.taluka_name
        AND gt."DistrictId" = d."DistrictId"
  );
