using Petroineos.IntraDayPosition.Core.Configuration;
using Petroineos.IntraDayPosition.Core.Service;

namespace Petroineos.IntraDayPosition.Service
{
    public class Worker : BackgroundService
    {
        private readonly IPositionService _positionService;
        private readonly IConfigProvider _config;
        private readonly ILogger<Worker> _logger;

        public Worker(
            IPositionService positionService,
            IConfigProvider config,
            ILogger<Worker> logger)
        {
            _positionService = positionService;
            _config = config;
            _logger = logger;
        }

        private async Task RunExtractAsync()
        {
            try
            {
                var ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                DateTime extractTime = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, ukTimeZone).DateTime;

                _logger.LogInformation("Starting scheduled extract for {time}", extractTime);

                await _positionService.ProcessPositionsAsync(extractTime, _config.CsvOutputPath);

                _logger.LogInformation("Extract successful. File saved to {path}", _config.CsvOutputPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the power position extract.");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("IntraDay Position Service started at: {time}", DateTimeOffset.Now);

            // An extract must run when the service first starts
            await RunExtractAsync();

            var interval = TimeSpan.FromMinutes(_config.ExtractIntervalInMinutes);
            using var timer = new PeriodicTimer(interval);
            
            _logger.LogInformation("Waiting {X} minutes for next extract...", _config.ExtractIntervalInMinutes);

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await RunExtractAsync();
                
                _logger.LogInformation("Waiting {X} minutes for next extract...", _config.ExtractIntervalInMinutes);
            }
        }
    }
}
