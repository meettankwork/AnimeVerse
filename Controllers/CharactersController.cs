
using AnimeVerse.Data;
using AnimeVerse.DTOs;
using AnimeVerse.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeVerse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CharactersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacterResponseDTOs>>> getCharacters()
        {
            var charc = await _context.Characters.Select(c => new CharacterResponseDTOs
            {
                CharacterId = c.CharacterId,
                CharacterName = c.CharacterName,
                Age = c.Age,
                Role = c.Role,
                PowerLevel = c.PowerLevel,
                AnimeId = c.AnimeId,
                AnimeName = c.Anime.AnimeName
            }).ToListAsync();

            return Ok(charc);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterResponseDTOs>> getCharacterById(int id)
        {
            var character = await _context.Characters.Include(c => c.Anime).SingleOrDefaultAsync(c => c.CharacterId == id);
            if (character == null)
            {
                return NotFound();
            }

            var resp = new CharacterResponseDTOs
            {
                Age = character.Age,
                AnimeId = character.AnimeId,
                AnimeName = character.Anime.AnimeName,
                Role = character.Role,
                CharacterId = character.CharacterId,
                CharacterName = character.CharacterName,
                PowerLevel = character.PowerLevel,
            };

            return Ok(resp);
        }

        [HttpPost]
        public async Task<ActionResult<CharacterResponseDTOs>> createCharacter(AddCharacter characterDto)
        {
            var anime = await _context.Animes.FindAsync(characterDto.AnimeId);
            if (anime == null)
            {
                return BadRequest("Anime with the provided ID does not exist.");
            }

            var Character = new CharactersModel
            {
                Age = characterDto.Age,
                AnimeId = characterDto.AnimeId,
                CreatedAt = DateTime.UtcNow,
                CharacterName = characterDto.CharacterName,
                PowerLevel = characterDto.PowerLevel,
                Role = characterDto.Role,
            };

            await _context.Characters.AddAsync(Character);
            await _context.SaveChangesAsync();

            var charResp = new CharacterResponseDTOs
            {
                CharacterId = Character.CharacterId,
                CharacterName = Character.CharacterName,
                Age = Character.Age,
                Role = Character.Role,
                PowerLevel = Character.PowerLevel,
                AnimeId = Character.AnimeId,
                AnimeName = anime.AnimeName,
            };

            return CreatedAtAction(nameof(getCharacterById), new { id = Character.CharacterId }, charResp);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CharacterResponseDTOs>> updateCharacters(int id, AddCharacter updateCharacter)
        {
            var anime = await _context.Animes.FindAsync(updateCharacter.AnimeId);
            if (anime == null)
            {
                return BadRequest("Anime with the provided ID does not exist.");
            }

            var findCharc = await _context.Characters.FindAsync(id);
            if (findCharc == null)
            {
                return NotFound("Character Not Found");
            }

            var Charc = new AddCharacter
            {
                Age = updateCharacter.Age,
                AnimeId = updateCharacter.AnimeId,
                CharacterName = updateCharacter.CharacterName,
                PowerLevel = updateCharacter.PowerLevel,
                Role = updateCharacter.Role
            };

            _context.Entry(findCharc).CurrentValues.SetValues(Charc);
            await _context.SaveChangesAsync();

            var charResp = new CharacterResponseDTOs
            {
                CharacterId = findCharc.CharacterId,
                CharacterName = findCharc.CharacterName,
                Age = findCharc.Age,
                Role = findCharc.Role,
                PowerLevel = findCharc.PowerLevel,
                AnimeId = findCharc.AnimeId,
                AnimeName = anime.AnimeName,
            };

            return Ok(charResp);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteCharacter(int id)
        {
            var findCharc = await _context.Characters.FindAsync(id);
            if (findCharc == null)
            {
                return NotFound();
            }
            _context.Characters.Remove(findCharc);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/abilites")]
        public async Task<ActionResult<CharacterAbilitiesDTOs>> getCharAbility(int id)
        { 
            var chars = await _context.Characters.
                Include(a => a.Abilities)
                .FirstOrDefaultAsync(c => c.CharacterId == id);

            if (chars == null) 
            {
                return BadRequest();
            }

            var charResp = new CharacterAbilitiesDTOs
            {
                CharacterName = chars.CharacterName,
                Abilities = chars.Abilities.Select(a => a.AbilityName).ToList()
            };

            return Ok(charResp);
        }
    }

}

