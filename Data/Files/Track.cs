namespace Doppler.Data.Files
{
    public class Track
    {
        public string FilePath { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Album { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int TrackNumber { get; set; }
        public int DiscNumber { get; set; }
        public int Year { get; set; }
        public bool IsCompilation { get; set; }
    }
}
