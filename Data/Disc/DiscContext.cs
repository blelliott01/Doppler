using Doppler.Data.Interfaces;

using TagLib;

namespace Doppler.Data.Disc
{
    public class DiscContext : ILibraryProvider
    {
        public async Task<IEnumerable<Artist>> LoadLibraryAsync(string rootPath)
        {
            return await Task.Run(() =>
            {
                List<Artist> artists = [];

                foreach (string artistDir in Directory.GetDirectories(rootPath))
                {
                    Artist artist = new() { Name = Path.GetFileName(artistDir) };

                    foreach (string albumDir in Directory.GetDirectories(artistDir))
                    {
                        Album album = new()
                        {
                            Artist = artist.Name,
                            Name = Path.GetFileName(albumDir)
                        };

                        foreach (string file in Directory.GetFiles(albumDir, "*.m4a"))
                        {
                            try
                            {
                                TagLib.File tagFile = TagLib.File.Create(file);
                                Tag tag = tagFile.Tag;
                                album.Tracks.Add(new Track
                                {
                                    FilePath = file,
                                    Artist = tag.FirstPerformer ?? artist.Name,
                                    Album = tag.Album ?? album.Name,
                                    Title = tag.Title ?? Path.GetFileNameWithoutExtension(file),
                                    TrackNumber = (int)tag.Track,
                                    DiscNumber = (int)tag.Disc,
                                    Year = (int)tag.Year,
                                    IsCompilation = tag.AlbumArtists.Contains("Various Artists")
                                });
                            }
                            catch { /* Skip unreadable files */ }
                        }

                        if (album.Tracks.Count != 0)
                        {
                            artist.Albums.Add(album);
                        }
                    }

                    if (artist.Albums.Count != 0)
                    {
                        artists.Add(artist);
                    }
                }

                return artists;
            });
        }
    }
}
