namespace APImom4.Models;

public class CategoryModel
{
    // Properties
    public int Id { get; set; }

    public string? Name { get; set; }

    // Kategori kan kan flera låtar
    public List<SongModel>? Songs { get; set; }
}
