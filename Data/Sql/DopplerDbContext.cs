using Doppler.Data.Disc;
using Doppler.Data.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Doppler.Data.Sql
{
    public class DopplerDbContext : DbContext, ILibraryRepository
    {
        public DbSet<TrackEntity> Tracks => Set<TrackEntity>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSqlite("Data Source=doppler.db");
        }

        public async Task SaveLibraryAsync(IEnumerable<Artist> artists)
        {
            _ = await Database.EnsureCreatedAsync();

            foreach (Artist artist in artists)
            {
                foreach (Album album in artist.Albums)
                {
                    foreach (Track track in album.Tracks)
                    {
                        if (!Tracks.Any(t => t.FilePath == track.FilePath))
                        {
                            _ = Tracks.Add(new TrackEntity
                            {
                                FilePath = track.FilePath,
                                Artist = track.Artist,
                                Album = track.Album,
                                Title = track.Title,
                                TrackNumber = track.TrackNumber,
                                DiscNumber = track.DiscNumber,
                                Year = track.Year,
                                IsCompilation = track.IsCompilation,
                                LastSeen = DateTime.UtcNow
                            });
                        }
                    }
                }
            }

            _ = await SaveChangesAsync();
        }
    }
}
