using Petroineos.IntraDayPosition.Core.Models;

namespace Petroineos.IntraDayPosition.Core.Storage
{
    public interface ICsvGenerator
    {
        void Generate(IEnumerable<HourlyPosition> positions, DateTime extractTime, string folderPath);
    }
}
