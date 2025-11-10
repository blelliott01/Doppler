using Doppler.Data.Disc;

namespace Doppler.Data.Interfaces
{
    public interface ILibraryRepository
    {
        Task SaveLibraryAsync(IEnumerable<Artist> artists);
    }
}
