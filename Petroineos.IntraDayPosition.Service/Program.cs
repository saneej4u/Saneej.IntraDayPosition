using Petroineos.IntraDayPosition.Core.Configuration;
using Petroineos.IntraDayPosition.Core.Service;
using Petroineos.IntraDayPosition.Core.Storage;
using Petroineos.IntraDayPosition.Service;
using Services;

var builder = Host.CreateApplicationBuilder(args);

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
