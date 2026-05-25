using System.ComponentModel.DataAnnotations;

namespace AnimeVerse.DTOs
{
    public class AnimeResponseDTOs
    {
        [Required]
        public int AnimeId { get; set; }
        [Required]
        public string AnimeName { get; set; }
        public int ReleaseYear { get; set; }
        public string Studio { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
    }
}
