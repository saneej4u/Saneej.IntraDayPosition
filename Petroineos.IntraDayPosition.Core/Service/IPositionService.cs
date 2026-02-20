namespace Petroineos.IntraDayPosition.Core.Service
{
    public interface IPositionService
    {
        Task ProcessPositionsAsync(DateTime runtime, string outputPath);
    }
}
