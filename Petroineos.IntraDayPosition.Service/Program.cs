using Petroineos.IntraDayPosition.Core.Configuration;
using Petroineos.IntraDayPosition.Core.Service;
using Petroineos.IntraDayPosition.Core.Storage;
using Petroineos.IntraDayPosition.Service;
using Serilog;
using Services;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Starting up the Petroineos IntraDay Position Service");
    var builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddSerilog();

    builder.Services.AddWindowsService(options =>
    {
        options.ServiceName = "Petroineos IntraDay Position Service";
    });

    builder.Services.AddSingleton<IConfigProvider, ConfigProvider>();
    builder.Services.AddTransient<ICsvGenerator, CsvGenerator>();

    builder.Services.AddSingleton<IPowerService, PowerService>();

    builder.Services.AddTransient<IPositionService, PositionService>();

    builder.Services.AddHostedService<Worker>();

    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Service terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
