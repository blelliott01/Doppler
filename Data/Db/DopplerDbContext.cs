using Doppler.Data.Files;
using Doppler.Data.Interfaces;
using Doppler.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Doppler.Data.Db
{
    public class DopplerDbContext(DbContextOptions<DopplerDbContext> options) : DbContext(options), ILibraryRepository
    {
        public DbSet<MediaFile> MediaFiles { get; set; }

        public Task SaveLibraryAsync(IEnumerable<Artist> artists)
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<MediaFile>()
                .HasIndex(m => m.Path)
                .IsUnique();
        }
    }
}
