using System.ComponentModel.DataAnnotations;

namespace RazorPagesMovie.Models;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation property
    public ICollection<Movie>? Movies { get; set; }
}
