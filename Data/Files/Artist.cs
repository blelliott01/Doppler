namespace Doppler.Data.Files
{
    public class Artist
    {
        public string Name { get; set; } = string.Empty;
        public List<Album> Albums { get; set; } = [];
    }
}
