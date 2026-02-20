using Microsoft.Extensions.Configuration;

namespace Petroineos.IntraDayPosition.Core.Configuration
{

    public class ConfigProvider : IConfigProvider
    {
        private readonly IConfiguration _configuration;

        public ConfigProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CsvOutputPath =>
            _configuration["TradeSettings:CsvOutputPath"] ?? "C:\\Exports\\";

        public int ExtractIntervalInMinutes =>
            int.TryParse(_configuration["TradeSettings:ExtractIntervalInMinutes"], out var x) ? x : 60;
    }
}
