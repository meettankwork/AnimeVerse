using AnimeVerse.Data;
using AnimeVerse.DTOs;
using AnimeVerse.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AnimeVerse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbilitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AbilitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AbilityResponseDTOs>>> getAbilities()
        {
            var abilities = await _context.Abilities.Select(a => new AbilityResponseDTOs
            {
                AbilityId = a.AbilityId,
                AbilityName = a.AbilityName,
                AbilityType = a.AbilityType,
                AnimeName = a.Character.Anime.AnimeName,
                CharacterId = a.CharacterId,
                CharacterName = a.Character.CharacterName,
                DamageLevel = a.DamageLevel,
            }).ToListAsync();

            return Ok(abilities);
        }
        // Get/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AbilityResponseDTOs>> getAbilitybyId(int id)
        {
            var ability = await _context.Abilities.Where(a => a.AbilityId == id).Select(a => new AbilityResponseDTOs
            {
                AbilityId = a.AbilityId,
                AbilityType = a.AbilityType,
                AbilityName = a.AbilityName,
                CharacterId = a.Character.CharacterId,
                CharacterName = a.Character.CharacterName,
                DamageLevel = a.DamageLevel,
                AnimeName = a.Character.Anime.AnimeName,
            }).FirstOrDefaultAsync();

            if (ability == null)
            {
                return BadRequest("Ability not found");
            }

            return Ok(ability);
        }

        // Post
        [HttpPost]
        public async Task<ActionResult<AbilityResponseDTOs>> addAbility(AddAbilityDTOs addAbility)
        {
            if (addAbility == null)
            {
                return BadRequest();
            }

            var newAbilit = new Abilities
            {
                AbilityName = addAbility.AbilityName,
                AbilityType = addAbility.AbilityType,
                DamageLevel = addAbility.DamageLevel,
                CharacterId = addAbility.CharacterId,
                CreatedAt = DateTime.UtcNow,
            };

            await _context.Abilities.AddAsync(newAbilit);
            await _context.SaveChangesAsync();

            var saveAbility = await _context.Abilities.
                Include(a => a.Character)
                .ThenInclude(c => c.Anime)
                .FirstOrDefaultAsync(a => a.AbilityId == newAbilit.AbilityId);

            var abilityResponse = new AbilityResponseDTOs
            {
                AbilityId = saveAbility.AbilityId,
                AbilityName = saveAbility.AbilityName,
                AbilityType = saveAbility.AbilityType,
                AnimeName = saveAbility.Character.Anime.AnimeName,
                CharacterId = saveAbility.CharacterId,
                CharacterName = saveAbility.Character.CharacterName,
                DamageLevel = saveAbility.DamageLevel,
            };

            return Ok(abilityResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AbilityResponseDTOs>> updateAbility(int id, AddAbilityDTOs updateAbility)
        {
            var ability = await _context.Abilities.FindAsync(id);
            if (ability == null)
            {
                return NotFound("Ability not found");
            }
            ability.AbilityName = updateAbility.AbilityName;
            ability.AbilityType = updateAbility.AbilityType;
            ability.DamageLevel = updateAbility.DamageLevel;
            ability.CharacterId = updateAbility.CharacterId;

            await _context.SaveChangesAsync();
            var updatedAbility = await _context.Abilities
                .Include(a => a.Character)
                .ThenInclude(c => c.Anime)
                .FirstOrDefaultAsync(a => a.AbilityId == id);

            var abilityResponse = new AbilityResponseDTOs
            {
                AbilityId = updatedAbility.AbilityId,
                AbilityName = updatedAbility.AbilityName,
                AbilityType = updatedAbility.AbilityType,
                AnimeName = updatedAbility.Character.Anime.AnimeName,
                CharacterId = updatedAbility.CharacterId,
                CharacterName = updatedAbility.Character.CharacterName,
                DamageLevel = updatedAbility.DamageLevel,
            };
            return Ok(abilityResponse);
        }

        // Delet/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteAbility(int id)
        {
            var ability = await _context.Abilities.FindAsync(id);
            if (ability == null)
            {
                return NotFound("Ability not found");
            }
            _context.Abilities.Remove(ability);
            await _context.SaveChangesAsync();
            return Ok("Ability deleted successfully");
        }
    }
}
