using System;
using agriApp.Entities.Lots;

namespace agriApp.Entities.Market
{
    public class Crop
    {
        public int CropId { get; set; }                 // PK - serial
        public string CropName { get; set; } = default!;

        public string? Grade { get; set; }              // optional
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
public List<PreRegisteredLot> Lots { get; set; } = new();

        public Crop() { }

        public Crop(string cropName, string? grade = null)
        {
            CropName = cropName;
            Grade = grade;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
