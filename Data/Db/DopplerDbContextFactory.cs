using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Doppler.Data.Db
{
    public class DopplerDbContextFactory : IDesignTimeDbContextFactory<DopplerDbContext>
    {
        public DopplerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DopplerDbContext>();
            _ = optionsBuilder.UseSqlite("Data Source=doppler.db");
            return new DopplerDbContext(optionsBuilder.Options);
        }
    }
}
