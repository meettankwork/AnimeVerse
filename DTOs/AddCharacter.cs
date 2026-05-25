using System.ComponentModel.DataAnnotations;

namespace AnimeVerse.DTOs
{
    public class AddCharacter
    {
        [Required]
        public string CharacterName { get; set; } = string.Empty;
        public int? Age { get; set; }
        public string Role { get; set; } = string.Empty;
        [Range(1,1000)]
        public int PowerLevel { get; set; }
        [Required]
        public int AnimeId { get; set; }
    }
}
