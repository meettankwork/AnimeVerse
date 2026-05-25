using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AnimeVerse.Model
{
    public class Abilities
    {
        [Key]
        public int AbilityId { get; set; }
        public string AbilityName { get; set; } = string.Empty;
        public string AbilityType { get; set; } = string.Empty;
        public int DamageLevel { get; set; }
        public int CharacterId { get; set; }
        public DateTime CreatedAt { get; set; }
        public CharactersModel Character { get; set; } = null!;
    }
}
