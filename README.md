# Petroineos IntraDay Position Service

This is a Windows Background Service built using .NET 8, designed to run continuously and extract power trade positional data recursively. 
It interacts with an external dependency (`PowerService.dll`) to aggregate and save trade volume positions correctly on to disk as CSV files.

## Configuration
The parameters are handled using standard `.NET IConfiguration` bindings.
In `Petroineos.IntraDayPosition.Service\appsettings.json`, you will find the `TradeSettings` block containing:

- `CsvOutputPath`: Controls where the extract files are saved (Defaults to: `C:\\TradingReports\\PowerPositions\\`).
- `ExtractIntervalInMinutes`: Represents the duration between scheduled exports in minutes (Defaults to: `1`).

## Logs
The service utilizes **Serilog** to generate rolling log streams. 

- Log files are generated and stored at: `C:\TradingReports\Logs\log-yyyyMMdd.txt`.