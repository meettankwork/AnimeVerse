using System.ComponentModel.DataAnnotations;

namespace AnimeVerse.DTOs
{
    public class AbilityResponseDTOs
    {
        [Required]
        public int AbilityId { get; set; }
        public string AbilityName { get; set; } = string.Empty;
        public string AbilityType { get; set; } = string.Empty;
        [Range(1, 1000)]
        public int DamageLevel { get; set; }
        [Required]
        public int CharacterId { get; set; }
        public string CharacterName { get; set; } = string.Empty;
        public string AnimeName { get; set; } = string.Empty;
    }
}
