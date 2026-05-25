using System.ComponentModel.DataAnnotations;

namespace AnimeVerse.DTOs
{
    public class CharacterResponseDTOs
    {
        [Required]
        public int CharacterId { get; set; }
        public string CharacterName { get; set; } = string.Empty;
        public int? Age { get; set; }
        public string Role { get; set; } = string.Empty;
        [Range(1,1000)]
        public int PowerLevel { get; set; }
        public int AnimeId { get; set; }
        public string AnimeName { get; set; } = string.Empty;
    }
}
