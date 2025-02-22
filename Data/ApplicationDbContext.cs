using Microsoft.EntityFrameworkCore;
using APImom4.Models;

namespace APImom4.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Tabellerna i databasen
    public DbSet<SongModel> Songs { get; set; }
    public DbSet<CategoryModel> Categories { get; set; }

    // Inställningar för relationerna mellan tabellerna
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SongModel>()
            .HasOne(s => s.Category)                // En sång har en kategori
            .WithMany(c => c.Songs)                 // En kategori har många sånger    
            .HasForeignKey(s => s.CategoryId)       // FK     
            .IsRequired(true);                      // Required  CategoryId i SongModel
    }
}
