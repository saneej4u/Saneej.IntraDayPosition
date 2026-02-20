using Microsoft.Extensions.Logging;
using Petroineos.IntraDayPosition.Core.Models;
using Petroineos.IntraDayPosition.Core.Storage;
using Services;

namespace Petroineos.IntraDayPosition.Core.Service
{
    public class PositionService : IPositionService
    {
        private readonly IPowerService _tradingSystem;
        private readonly ICsvGenerator _csvGenerator;
        private readonly ILogger<PositionService> _logger;

        public PositionService(IPowerService tradingSystem, ICsvGenerator csvGenerator, ILogger<PositionService> logger)
        {
            _tradingSystem = tradingSystem;
            _csvGenerator = csvGenerator;
            _logger = logger;
        }

        public async Task ProcessPositionsAsync(DateTime runtime, string outputPath)
        {
            var trades = await _tradingSystem.GetTradesAsync(runtime);
            var hourlyAggregates = Aggregate(trades);
            _csvGenerator.Generate(hourlyAggregates, runtime, outputPath);
        }

        internal IEnumerable<HourlyPosition> Aggregate(IEnumerable<Services.PowerTrade> trades)
        {
            var volumes = new double[24];

            foreach (var trade in trades)
            {
                foreach (var period in trade.Periods)
                {
                    if (period.Period >= 1 && period.Period <= 24)
                        volumes[period.Period - 1] += period.Volume;
                }
            }

            return volumes.Select((vol, index) =>
            {
                var hour = (index + 23) % 24;
                return new HourlyPosition($"{hour:D2}:00", vol);
            });
        }
    }
}
