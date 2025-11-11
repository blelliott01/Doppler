using Microsoft.EntityFrameworkCore;
using Doppler;
using Doppler.Data.Db;
using Doppler.Data.Files;
using Doppler.Services;
using Doppler.Data.Interfaces;

DbContextOptions<DopplerDbContext> options = new DbContextOptionsBuilder<DopplerDbContext>()
    .UseSqlite("Data Source=doppler.db")
    .Options;

ILibraryRepository repo = new DopplerDbContext(options);
FileContext provider = new();
LibraryService service = new(provider, repo);

await service.SyncAsync("/Volumes/T7/Doppler");
Console.WriteLine("Library sync complete!");

if (args.Contains("--tui"))
{
    DopplerTUI.Run();
    return;
}

Console.WriteLine("Doppler running in standard mode.");
Console.WriteLine("Use `dotnet run -- --tui` to launch the text UI.");
