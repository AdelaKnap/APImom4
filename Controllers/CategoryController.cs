using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APImom4.Data;
using APImom4.Models;

namespace APImom4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Category, hämta alla kategorier och ha med bara artist och titel för låtarna i get-anropet
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategories()
        {
            // Hämta alla kategorier och songs
            var categories = await _context.Categories
                .Include(c => c.Songs)                      // alla låtar
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    Songs = c.Songs != null ? c.Songs.Select(s => new { s.Artist, s.Title }) : null // Endast artist och titel
                })
                .ToListAsync();

            return Ok(categories);
        }


        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryModel>> GetCategoryModel(int id)
        {
            var categoryModel = await _context.Categories.FindAsync(id);

            if (categoryModel == null)
            {
                return NotFound();
            }

            return categoryModel;
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryModel(int id, CategoryModel categoryModel)
        {
            if (id != categoryModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(categoryModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Meddelande om att uppdatering har skett
            return Ok(new { message = $"Kategori med ID {id} har uppdaterats." });
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<CategoryModel>> PostCategoryModel(CategoryModel categoryModel)
        {
            _context.Categories.Add(categoryModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoryModel", new { id = categoryModel.Id }, categoryModel);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryModel(int id)
        {
            var categoryModel = await _context.Categories.FindAsync(id);
            if (categoryModel == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(categoryModel);
            await _context.SaveChangesAsync();

            // Meddelande om att kategori har tagits bort
            return Ok(new { message = $"Kategori med ID {id} har tagits bort, inklusive alla låtar kopplade till kategorin" });
        }

        private bool CategoryModelExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
