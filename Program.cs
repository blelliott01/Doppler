using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using Doppler;
using Doppler.Data.Db;
using Doppler.Data.Files;
using Doppler.Services;
using Doppler.Data.Interfaces;

// Parse command-line args early
bool verbose = args.Contains("--verbose");

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // 1️⃣ Register settings first
        services.AddSingleton(new DopplerSettings(verbose));

        // 2️⃣ EF Core
        services.AddDbContext<DopplerDbContext>(options =>
            options.UseSqlite("Data Source=doppler.db"));

        // 3️⃣ Doppler components
        services.AddScoped<ILibraryRepository, DopplerDbContext>();
        services.AddScoped<ILibraryProvider, FileContext>();
        services.AddScoped<LibraryService>();

        // Logging
        services.AddLogging(config =>
        {
            config.ClearProviders();
            config.AddSimpleConsole(options =>
            {
                options.SingleLine = true;
                options.TimestampFormat = "[HH:mm:ss] ";
            });

            // Default level
            config.SetMinimumLevel(verbose ? LogLevel.Debug : LogLevel.Information);

            if (verbose)
            {
                // In verbose mode, show everything including EF Core SQL
                config.AddFilter("Microsoft", LogLevel.Debug);
            }
            else
            {
                // Quiet mode: suppress EF and framework chatter
                config.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
                config.AddFilter("Microsoft.Hosting", LogLevel.Warning);
            }
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
