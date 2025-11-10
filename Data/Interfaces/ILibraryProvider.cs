using Doppler.Data.Disc;

namespace Doppler.Data.Interfaces
{
    public interface ILibraryProvider
    {
        Task<IEnumerable<Artist>> LoadLibraryAsync(string rootPath);
    }
}
