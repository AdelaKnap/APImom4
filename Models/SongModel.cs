using System.ComponentModel.DataAnnotations;

namespace APImom4.Models;

public class SongModel
{
    // Properties
    public int Id { get; set; }

    [Required]
    public string? Artist { get; set; }

    [Required]
    public string? Title { get; set; }

    [Required]
    public int? Length { get; set; }

    // FK till Category
    public int? CategoryId { get; set; }

    // Modellen för category
    public CategoryModel? Category { get; set; }
}