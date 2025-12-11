using System.Text;
using OfficeOpenXml;

namespace agriApp.Services.Templates
{
    public static class TemplateFactory
    {
        public static MemoryStream CreateCsvTemplate()
        {
            var csv = new StringBuilder();
            csv.AppendLine("FarmerName,MobileNumber,Location,ProfilePhotoUrl,InterestedCrops,PrimaryCrop,FarmLocation,FarmSize");

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(csv.ToString());
            writer.Flush();
            stream.Position = 0;

            return stream;
        }

        public static MemoryStream CreateExcelTemplate()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Template");

            sheet.Cells[1, 1].Value = "FarmerName";
            sheet.Cells[1, 2].Value = "MobileNumber";
            sheet.Cells[1, 3].Value = "Location";
            sheet.Cells[1, 4].Value = "ProfilePhotoUrl";
            sheet.Cells[1, 5].Value = "InterestedCrops";
            sheet.Cells[1, 6].Value = "PrimaryCrop";
            sheet.Cells[1, 7].Value = "FarmLocation";
            sheet.Cells[1, 8].Value = "FarmSize";

            var stream = new MemoryStream(package.GetAsByteArray());
            stream.Position = 0;
            return stream;
        }
    }
}
