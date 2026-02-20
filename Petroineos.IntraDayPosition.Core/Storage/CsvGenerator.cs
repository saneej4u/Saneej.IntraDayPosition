using Petroineos.IntraDayPosition.Core.Models;
using System.Text;

namespace Petroineos.IntraDayPosition.Core.Storage
{
    public class CsvGenerator : ICsvGenerator
    {
        public void Generate(IEnumerable<HourlyPosition> positions, DateTime extractTime, string folderPath)
        {
            string fileName = $"PowerPosition_{extractTime:yyyyMMdd_HHmm}.csv";
            string fullPath = Path.Combine(folderPath, fileName);

            var csv = new StringBuilder();
            csv.AppendLine("Local Time,Volume");

            foreach (var pos in positions)
            {
                csv.AppendLine($"{pos.LocalTime},{pos.Volume}");
            }

            File.WriteAllText(fullPath, csv.ToString());
        }
    }
}
