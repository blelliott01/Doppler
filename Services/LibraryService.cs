using Doppler.Data.Interfaces;

namespace Doppler.Services
{
    public class LibraryService(ILibraryProvider provider, ILibraryRepository repository)
    {
        private readonly ILibraryProvider _provider = provider;
        private readonly ILibraryRepository _repository = repository;

        public async Task SyncAsync(string rootPath)
        {
            IEnumerable<Data.Files.Artist> artists = await _provider.LoadLibraryAsync(rootPath);
            await _repository.SaveLibraryAsync(artists);
        }
    }
}
