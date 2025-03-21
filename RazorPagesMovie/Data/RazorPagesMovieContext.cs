using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Data;

public class RazorPagesMovieContext(DbContextOptions<RazorPagesMovieContext> options)
    : DbContext(options)
{
    public DbSet<Movie> Movie { get; set; } = default!;
    public DbSet<Genre> Genre { get; set; } = default!;
}
