    namespace agriApp.DTOs.Anchors
    {
        public class BulkFarmerGroupedDto
        {
            public string Mobile { get; set; } = default!;
            public string FarmerName { get; set; } = default!;
            public string? Location { get; set; }

            public List<BulkFarmerRowDto> GroupRows { get; set; } = new();

            public List<string> AllInterestedCrops { get; set; } = new();

            public List<BulkFarmInputDto> Farms { get; set; } = new();

            public static List<BulkFarmerGroupedDto> GroupByMobile(List<BulkFarmerRowDto> rows)
            {
                return rows
                    .GroupBy(r => r.Mobile)
                    .Select(g => new BulkFarmerGroupedDto
                    {
                        Mobile = g.Key,
                        FarmerName = g.First().FarmerName,
                        Location = g.First().Location,
                        GroupRows = g.ToList(),
                        // AllInterestedCrops = g.SelectMany(x => x.InterestedCrops).ToList(),
// FIX: Remove duplicates across rows
                    AllInterestedCrops = g
                        .SelectMany(x => x.InterestedCrops)
                        .Where(c => !string.IsNullOrWhiteSpace(c))
                        .Select(c => c.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList(),
                        Farms = g.Select(x => new BulkFarmInputDto
                        {
                            FarmLocation = x.FarmLocation,
                            PrimaryCrop = x.PrimaryCrop,
                            FarmSize = x.FarmSize
                        }).ToList()
                    })
                    .ToList();
            }
        }

        public class BulkFarmInputDto
        {
            public string FarmLocation { get; set; } = default!;
            public string PrimaryCrop { get; set; } = default!;
            public float FarmSize { get; set; }
        }
    }
