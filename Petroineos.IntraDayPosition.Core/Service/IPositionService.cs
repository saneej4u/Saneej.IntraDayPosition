namespace Petroineos.IntraDayPosition.Core.Service
{
    internal interface IPositionService
    {
        Task ProcessPositionsAsync(DateTime runtime, string outputPath);
    }
}
