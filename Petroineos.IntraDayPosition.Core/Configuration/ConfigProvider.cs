using System.Configuration;

namespace Petroineos.IntraDayPosition.Core.Configuration
{
    public class ConfigProvider : IConfigProvider
    {
        public string CsvOutputPath => ConfigurationManager.AppSettings["CsvOutputPath"] ?? "C:\\Exports\\"; 
        public int ExtractIntervalInMinutes => int.TryParse(ConfigurationManager.AppSettings["ExtractIntervalInMinutes"], out var x) ? x : 60;
    }
}
