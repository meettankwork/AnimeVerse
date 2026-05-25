using System.ComponentModel.DataAnnotations;

namespace AnimeVerse.DTOs
{
    public class AddAbilityDTOs
    {
        [Required]
        [MaxLength(100)]
        public string AbilityName { get; set; } = string.Empty;
        public string AbilityType { get; set; } = string.Empty;
        [Range(1,1000)]
        public int DamageLevel { get; set; }
        public int CharacterId { get; set; }
    }
}
