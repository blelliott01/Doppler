namespace Doppler.Data.Disc
{
    public class Album
    {
        public string Artist { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<Track> Tracks { get; set; } = [];
    }
}
