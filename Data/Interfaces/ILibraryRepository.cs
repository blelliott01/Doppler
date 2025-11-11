using Doppler.Data.Files;

namespace Doppler.Data.Interfaces
{
    public interface ILibraryRepository
    {
        Task SaveLibraryAsync(IEnumerable<Artist> artists);
    }
}
