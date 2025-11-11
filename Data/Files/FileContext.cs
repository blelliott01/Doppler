using Doppler.Data.Interfaces;
using Doppler.Data.Models;
using Doppler.Data.Db;
using Microsoft.EntityFrameworkCore;

namespace Doppler.Data.Files
{
    public class FileContext : ILibraryProvider
    {
        public record FileSnapshot(string Path, DateTime ModifiedUtc, long Size);

        public static List<FileSnapshot> Scan(string rootPath)
        {
            List<FileSnapshot> results = [.. Directory
                .EnumerateFiles(rootPath, "*.m4a", SearchOption.AllDirectories)
                .Select(p => new FileSnapshot(
                    Path: p,
                    ModifiedUtc: File.GetLastWriteTimeUtc(p),
                    Size: new FileInfo(p).Length))];

            Console.WriteLine($"Scan found {results.Count} files under {rootPath}");
            return results;
        }

        // keep this for now, will replace in step 2
        public async Task<IEnumerable<Artist>> LoadLibraryAsync(string rootPath)
        {
            List<FileSnapshot> snapshot = await Task.Run(() => Scan(rootPath));

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
            // Step 1 – take new filesystem snapshot
            List<FileSnapshot> snapshot = Scan(rootPath);

            // Step 2 – load current DB entries (assumes MediaFiles table has Path, ModifiedUtc, Size)
            Dictionary<string, MediaFile> known = db.MediaFiles.AsNoTracking().ToDictionary(m => m.Path, m => m);

            // Step 3 – compute deltas
            List<FileSnapshot> added = [.. snapshot.Where(f => !known.ContainsKey(f.Path))];

            List<FileSnapshot> changed = [.. snapshot
                .Where(f => known.TryGetValue(f.Path, out MediaFile? k)
                         && (k.ModifiedUtc != f.ModifiedUtc || k.Size != f.Size))];

            List<MediaFile> deleted = [.. known.Values.Where(m => !snapshot.Any(f => f.Path == m.Path))];

            // Step 4 – report
            Console.WriteLine($"Added: {added.Count}, Changed: {changed.Count}, Deleted: {deleted.Count}");

            return new DeltaResult(added, changed, deleted);
        }
    }
}
