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
            _logger.LogInformation("Retrieving trades for {runtime}", runtime);
            var trades = await _tradingSystem.GetTradesAsync(runtime);
            
            var tradesList = trades?.ToList() ?? new List<Services.PowerTrade>();
            _logger.LogInformation("Retrieved {count} trades.", tradesList.Count);

            var hourlyAggregates = Aggregate(tradesList);
            _logger.LogInformation("Aggregated trades into {count} hourly periods. Generating CSV...", hourlyAggregates.Count());

            _csvGenerator.Generate(hourlyAggregates, runtime, outputPath);
            _logger.LogInformation("CSV Generation completed.");
        }

        internal IEnumerable<HourlyPosition> Aggregate(IEnumerable<Services.PowerTrade> trades)
        {
             var volumes = trades.SelectMany(t => t.Periods)
                                 .GroupBy(p => p.Period)
                                 .ToDictionary(g => g.Key, g => g.Sum(p => p.Volume));

             var numPeriods = volumes.Keys.Any() ? Math.Max(24, volumes.Keys.Max()) : 24;

             for (int i = 1; i <= numPeriods; i++)
             {
                 var vol = volumes.TryGetValue(i, out var v) ? v : 0.0;
                 var hour = (i + 22) % 24;
                 yield return new HourlyPosition($"{hour:D2}:00", vol);
             }
        }
    }
}
