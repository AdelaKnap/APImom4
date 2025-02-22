using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APImom4.Data;
using APImom4.Models;

namespace APImom4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SongController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Song, inklusive namn och id på kategori
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongModel>>> GetSongs()
        {
            // Hämta alla låtar från databasen, inklusive kategori
            return Ok(await _context.Songs.Include(s => s.Category).Select(s => new
            {
                s.Id,
                s.Artist,
                s.Title,
                s.Length,
                s.CategoryId,
                CategoryName = s.Category != null ? s.Category.Name : null
            }).ToListAsync());
        }


        // GET: api/Song/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SongModel>> GetSongModel(int id)
        {
            var songModel = await _context.Songs.FindAsync(id);

            if (songModel == null)
            {
                return NotFound();
            }

            return songModel;
        }

        // PUT: api/Song/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSongModel(int id, SongModel songModel)
        {
            if (id != songModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(songModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Meddelande om att uppdatering har skett
            return Ok(new { message = $"Sång med ID {id} har uppdaterats." });
        }

        // POST: api/Song
        [HttpPost]
        public async Task<ActionResult<SongModel>> PostSongModel(SongModel songModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kontrollera om kategori finns i databasen
            var category = await _context.Categories.FindAsync(songModel.CategoryId);
            if (category == null)
            {
                return BadRequest($"Kategori med id: {songModel.CategoryId} finns inte");
            }

            // Lägg till låten och koppla kategorin genom CategoryId
            _context.Songs.Add(songModel);
            await _context.SaveChangesAsync();

            // Returnera den nya låten, inklusive kategorin
            return CreatedAtAction("GetSongModel", new { id = songModel.Id }, songModel);
        }


        // DELETE: api/Song/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSongModel(int id)
        {
            var songModel = await _context.Songs.FindAsync(id);
            if (songModel == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(songModel);
            await _context.SaveChangesAsync();

            // Meddelande om att låten har tagits bort
            return Ok(new { message = $"Sång med ID {id} har tagits bort." });
        }

        private bool SongModelExists(int id)
        {
            return _context.Songs.Any(e => e.Id == id);
        }
    }
}
