using Doppler;
using Doppler.Data.Disc;
using Doppler.Data.Sql;
using Doppler.Services;

DiscContext provider = new();
DopplerDbContext repo = new();
LibraryService service = new(provider, repo);

await service.SyncAsync("/Volumes/T7/Doppler");
Console.WriteLine("Library sync complete!");
if (args.Contains("--tui"))
{
    DopplerTUI.Run();
    return;
}

// --- Normal Doppler console logic ---
Console.WriteLine("Doppler running in standard mode.");
Console.WriteLine("Use `dotnet run -- --tui` to launch the text UI.");
