using agriApp.DTOs.Anchors;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Globalization;

namespace agriApp.Services.BulkImport
{
    public class BulkFileParserService : IBulkFileParserService
    {
        public async Task<List<BulkFarmerRowDto>> ParseAsync(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLower();

            return ext switch
            {
                ".csv" => await ParseCsvAsync(file),
                ".xls" or ".xlsx" => await ParseExcelAsync(file),
                _ => throw new Exception("Unsupported file format. Only CSV and Excel are allowed.")
            };
        }

        // ---------------------------------------------------------
        // CSV PARSER
        // ---------------------------------------------------------
        private async Task<List<BulkFarmerRowDto>> ParseCsvAsync(IFormFile file)
        {
            var rows = new List<BulkFarmerRowDto>();

            using var reader = new StreamReader(file.OpenReadStream());
            int rowNum = 0;

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                rowNum++;

                if (rowNum == 1) continue; // skip header

                var cols = line.Split(',');

                if (cols.Length < 8)
                    throw new Exception($"Row {rowNum} has missing columns.");

                rows.Add(new BulkFarmerRowDto
                {
                    RowNumber = rowNum,
                    FarmerName = cols[0].Trim(),
                    Mobile = cols[1].Trim(),
                    Location = cols[2].Trim(),
                    ProfilePhotoUrl = string.IsNullOrWhiteSpace(cols[3]) ? null : cols[3].Trim(),
                    InterestedCrops = cols[4].Split('|', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList(),
                    FarmLocation = cols[5].Trim(),
                    PrimaryCrop = cols[6].Trim(),
                    FarmSize = float.Parse(cols[7], CultureInfo.InvariantCulture)
                });
            }

            return rows;
        }

        // ---------------------------------------------------------
        // EXCEL PARSER
        // ---------------------------------------------------------
        private async Task<List<BulkFarmerRowDto>> ParseExcelAsync(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var rows = new List<BulkFarmerRowDto>();

            using var package = new ExcelPackage(file.OpenReadStream());
            // await package.LoadAsync(file.OpenReadStream());

            var ws = package.Workbook.Worksheets[0];
            int rowCount = ws.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                rows.Add(new BulkFarmerRowDto
                {
                    RowNumber = row,
                    FarmerName = ws.Cells[row, 1].Text.Trim(),
                    Mobile = ws.Cells[row, 2].Text.Trim(),
                    Location = ws.Cells[row, 3].Text.Trim(),
                    ProfilePhotoUrl = ws.Cells[row, 4].Text.Trim(),
                    InterestedCrops = ws.Cells[row, 5].Text.Split('|', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList(),
                    FarmLocation = ws.Cells[row, 6].Text.Trim(),
                    PrimaryCrop = ws.Cells[row, 7].Text.Trim(),
                    FarmSize = float.Parse(ws.Cells[row, 8].Text.Trim(), CultureInfo.InvariantCulture)
                });
            }

            return rows;
        }
    }
}
