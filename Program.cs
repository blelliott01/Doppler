using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using Doppler;
using Doppler.Data.Db;
using Doppler.Data.Files;
using Doppler.Services;
using Doppler.Data.Interfaces;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // EF Core with SQLite
        _ = services.AddDbContext<DopplerDbContext>(options =>
            options.UseSqlite("Data Source=doppler.db"));

        // Doppler components
        _ = services.AddScoped<ILibraryRepository, DopplerDbContext>();
        _ = services.AddScoped<ILibraryProvider, FileContext>();
        _ = services.AddScoped<LibraryService>();

        // Logging
        _ = services.AddLogging(config =>
        {
            _ = config.ClearProviders();
            _ = config.AddSimpleConsole(options =>
            {
                options.SingleLine = true;
                options.TimestampFormat = "[HH:mm:ss] ";
            });

            _ = config.SetMinimumLevel(LogLevel.Information);
            _ = config.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
        });
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var db = scope.ServiceProvider.GetRequiredService<DopplerDbContext>();

    var pending = db.Database.GetPendingMigrations().ToList();

    if (pending.Count > 0)
    {
        logger.LogInformation("Applying {Count} pending migration{Suffix}...",
            pending.Count, pending.Count == 1 ? "" : "s");

        db.Database.Migrate();

        logger.LogInformation("Migrations complete!");
    }
    else
    {
        logger.LogInformation("Database up-to-date (no pending migrations).");
    }
}

var service = host.Services.GetRequiredService<LibraryService>();
var logger2 = host.Services.GetRequiredService<ILogger<Program>>();

logger2.LogInformation("Doppler starting...");

await service.SyncAsync("/Volumes/T7/Doppler");
logger2.LogInformation("Library sync complete!");

if (args.Contains("--tui"))
{
    DopplerTUI.Run();
    return;
}

logger2.LogInformation("Doppler running in standard mode. Use `dotnet run -- --tui` to launch the text UI.");
