using Doppler.Data.Interfaces;
using Doppler.Data.Models;
using Doppler.Data.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Doppler.Data.Files
{
    public class FileContext(ILogger<FileContext> logger, DopplerSettings settings) : ILibraryProvider
    {
        private readonly ILogger<FileContext> _logger = logger;
        private readonly DopplerSettings _settings = settings;

        public record FileSnapshot(string Path, DateTime ModifiedUtc, long Size);

        public static List<FileSnapshot> Scan(string rootPath)
        {
            List<FileSnapshot> results = [.. Directory
                .EnumerateFiles(rootPath, "*.m4a", SearchOption.AllDirectories)
                .Select(p => new FileSnapshot(
                    Path: p,
                    ModifiedUtc: File.GetLastWriteTimeUtc(p),
                    Size: new FileInfo(p).Length))];

            return results;
        }

        public async Task<IEnumerable<Artist>> LoadLibraryAsync(string rootPath)
        {
            if (_settings.Verbose)
                _logger.LogInformation("Scanning {Root} for media files...", rootPath);

            List<FileSnapshot> snapshot = await Task.Run(() => Scan(rootPath));

            if (_settings.Verbose)
                _logger.LogInformation("Found {Count} .m4a files under {Root}", snapshot.Count, rootPath);

            // just return an empty artist list for now to satisfy ILibraryProvider
            return [];
        }

        public record DeltaResult(
            List<FileSnapshot> Added,
            List<FileSnapshot> Changed,
            List<MediaFile> Deleted
        );

        public static DeltaResult CompareToDatabase(DopplerDbContext db, string rootPath)
        {
            List<FileSnapshot> snapshot = Scan(rootPath);
            Dictionary<string, MediaFile> known =
                db.MediaFiles.AsNoTracking().ToDictionary(m => m.Path, m => m);

            List<FileSnapshot> added = [.. snapshot.Where(f => !known.ContainsKey(f.Path))];

            List<FileSnapshot> changed = [.. snapshot
                .Where(f => known.TryGetValue(f.Path, out MediaFile? k)
                         && (k.ModifiedUtc != f.ModifiedUtc || k.Size != f.Size))];

            List<MediaFile> deleted = [.. known.Values.Where(m => !snapshot.Any(f => f.Path == m.Path))];

            return new DeltaResult(added, changed, deleted);
        }
    }
}
