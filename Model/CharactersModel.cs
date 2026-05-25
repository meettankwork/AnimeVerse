using System.ComponentModel.DataAnnotations;

namespace AnimeVerse.Model
{
    public class CharactersModel
    {
        [Key]
        public int CharacterId { get; set; }            
        public string CharacterName { get; set; } = string.Empty;
        public int? Age { get; set; } 
        public string Role { get; set; } = string.Empty;
        public int PowerLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AnimeId { get; set; }
        public Anime Anime { get; set; } = null!;
        public List<Abilities> Abilities { get; set; } = new List<Abilities>();
    }
}
