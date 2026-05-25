namespace AnimeVerse.Model
{
    public class Anime
    {
        public int AnimeId { get; set; }
        public string AnimeName { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public string Studio { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public ICollection<CharactersModel> CharactersNames { get; set; } = new List<CharactersModel>();

    }
}
