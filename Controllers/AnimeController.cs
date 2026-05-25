using AnimeVerse.DTOs;
using AnimeVerse.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnimeVerse.Model;

namespace AnimeVerse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AnimeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimeResponseDTOs>>> getAnime()
        {
            var anime = await _context.Animes.Select(a => new AnimeResponseDTOs
            {
                AnimeId = a.AnimeId,
                AnimeName = a.AnimeName,
                ReleaseYear = a.ReleaseYear,
                Studio = a.Studio,
                Genre = a.Genre
            }).ToListAsync();

            return Ok(anime);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnimeResponseDTOs>> getAnimeById(int id)
        {
            var anime = await _context.Animes.Where(e => e.AnimeId == id).
                Select(e => new AnimeResponseDTOs
                {
                    AnimeId = e.AnimeId,
                    AnimeName = e.AnimeName,
                    Studio = e.Studio,
                    ReleaseYear = e.ReleaseYear,
                    Genre = e.Genre
                }).FirstOrDefaultAsync();

            if (anime == null) 
            {
                return NotFound();
            }
            return Ok(anime);
        }

        [HttpPost]
        public async Task<ActionResult<AnimeResponseDTOs>> addAnime(AddAnime addAnime)
        {

            if(addAnime == null)
            {
                return BadRequest("Something Went Wrong");
            }

            var anime = new Anime
            {               
                AnimeName = addAnime.AnimeName,
                ReleaseYear = addAnime.ReleaseYear,
                Studio = addAnime.Studio,
                Genre = addAnime.Genre,
                CreatedAt = DateTime.UtcNow
            };          

            await _context.Animes.AddAsync(anime);
            await _context.SaveChangesAsync();

            var respAnime = new AnimeResponseDTOs
            {
                AnimeId = anime.AnimeId,
                AnimeName = anime.AnimeName,
                ReleaseYear = anime.ReleaseYear,
                Studio = anime.Studio,
                Genre = anime.Genre
            };

            return Ok(respAnime);

        }


        [HttpPut("{id}")]
        public async Task<ActionResult<AnimeResponseDTOs>> updateAnime(int id, AddAnime updateAnime)
        {
            var anime = await _context.Animes.FindAsync(id);

            if (anime == null)
            {
                return NotFound("Anime Dosent Exist");
            }
            var animeRe = new AnimeResponseDTOs
            {
                AnimeId = anime.AnimeId,
                AnimeName = updateAnime.AnimeName,
                ReleaseYear = updateAnime.ReleaseYear,
                Studio = updateAnime.Studio,
                Genre = updateAnime.Genre,
            };

             _context.Entry(anime).CurrentValues.SetValues(updateAnime);
            await _context.SaveChangesAsync();

            return Ok(animeRe);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteAnime(int id)
        {
            var anime = await _context.Animes.FindAsync(id);
            if (anime == null)
            {
                return NotFound("Anime Doesn't Exist");
            }

            _context.Animes.Remove(anime);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("{id}/charachters")]
        public async Task<ActionResult<AnimeCharacters>> getAnimeCharacters(int id) 
        {
            var anime = await _context.Animes.Include(c => c.CharactersNames).FirstOrDefaultAsync(a => a.AnimeId == id);
            if (anime == null) 
            {
                return NotFound("Anime Doesn't Exist");
            }

            var aniCharREsp = new AnimeCharacters
            {
                AnimeName = anime.AnimeName,
                Characters = anime.CharactersNames.Select(c => c.CharacterName).ToList()
            };
            if (aniCharREsp.Characters == null || aniCharREsp.Characters.Count == 0) 
            {
                return NotFound("No Characters Found For This Anime");
            }
            return Ok(aniCharREsp);
        }

        [HttpGet("{id}/characters/abilities")]
        public async Task<ActionResult<AnimeCharacterAbilitiesDTO>> GetAnimeCharacterAbilities(int id)
        {
            var anime = await _context.Animes
       .Include(a => a.CharactersNames)
       .ThenInclude(c => c.Abilities)
       .FirstOrDefaultAsync(a => a.AnimeId == id);

            if (anime == null)
            {
                return NotFound("Anime not found");
            }

            var response = new AnimeCharacterAbilitiesDTO
            {
                AnimeName = anime.AnimeName,

                Characters = anime.CharactersNames
                    .Select(c => new CharacterAbilitiesDTOs
                    {
                        CharacterName = c.CharacterName,

                        Abilities = c.Abilities
                            .Select(a => a.AbilityName)
                            .ToList()
                    })
                    .ToList()
            };

            return Ok(response);
        }

    }
}
