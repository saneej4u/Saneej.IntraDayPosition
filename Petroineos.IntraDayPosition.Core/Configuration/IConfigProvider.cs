namespace Petroineos.IntraDayPosition.Core.Configuration
{
    public interface IConfigProvider
    {
        public string CsvOutputPath { get; }
        public int ExtractIntervalInMinutes { get; }
    }
}
