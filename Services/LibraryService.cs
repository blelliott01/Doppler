using Doppler.Data.Interfaces;

using Microsoft.Extensions.Logging;

namespace Doppler.Services
{
    public class LibraryService
    {
        private readonly ILibraryProvider _provider;
        private readonly ILibraryRepository _repository;
        private readonly ILogger<LibraryService> _logger;

        public LibraryService(ILibraryProvider provider, ILibraryRepository repository, ILogger<LibraryService> logger)
        {
            _provider = provider;
            _repository = repository;
            _logger = logger;
        }

        public async Task SyncAsync(string rootPath)
        {
            _logger.LogInformation("Starting library sync for {Path}", rootPath);
            // existing sync logic
            _logger.LogInformation("Library sync finished");
        }
    }
}
