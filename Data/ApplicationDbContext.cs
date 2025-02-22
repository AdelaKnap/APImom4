using Microsoft.EntityFrameworkCore;
using APImom4.Models;

namespace APImom4.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Tabellerna i databasen
    public DbSet<SongModel>? Songs { get; set; }
    public DbSet<CategoryModel>? Categories { get; set; }
}
