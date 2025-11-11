namespace Doppler.Data.Models
{
    public class MediaFile
    {
        public int Id { get; set; }
        public string Path { get; set; } = "";
        public DateTime ModifiedUtc { get; set; }
        public long Size { get; set; }
    }
}
