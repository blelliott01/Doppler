using Doppler.Data.Files;

namespace Doppler.Data.Interfaces
{
    public interface ILibraryProvider
    {
        Task<IEnumerable<Artist>> LoadLibraryAsync(string rootPath);
    }
}
