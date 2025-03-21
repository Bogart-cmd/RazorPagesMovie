using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new RazorPagesMovieContext(
            serviceProvider.GetRequiredService<DbContextOptions<RazorPagesMovieContext>>());

        if (context.Genre.Any() || context.Movie.Any())
        {
            // DB has been seeded already
            return;
        }

        // Add Genres
        var genres = new List<Genre>
        {
            new() { Name = "Action" },
            new() { Name = "Comedy" },
            new() { Name = "Horror" },
            new() { Name = "Sci-Fi" },
            new() { Name = "Drama" },
            new() { Name = "Thriller" },
            new() { Name = "Fantasy" }
        };

        context.Genre.AddRange(genres);
        context.SaveChanges();

        // Movies
        context.Movie.AddRange(
            new()
            {
                Title = "John Wick 4",
                ReleaseDate = DateTime.Parse("2023-03-24"),
                Price = 14.99M,
                GenreId = genres.First(g => g.Name == "Action").Id
            },
            new()
            {
                Title = "Dune: Part Two",
                ReleaseDate = DateTime.Parse("2024-03-01"),
                Price = 19.99M,
                GenreId = genres.First(g => g.Name == "Sci-Fi").Id
            },
            new()
            {
                Title = "Oppenheimer",
                ReleaseDate = DateTime.Parse("2023-07-21"),
                Price = 15.99M,
                GenreId = genres.First(g => g.Name == "Drama").Id
            },
            new()
            {
                Title = "Barbie",
                ReleaseDate = DateTime.Parse("2023-07-21"),
                Price = 12.99M,
                GenreId = genres.First(g => g.Name == "Comedy").Id
            },
            new()
            {
                Title = "A Quiet Place: Day One",
                ReleaseDate = DateTime.Parse("2024-06-28"),
                Price = 13.99M,
                GenreId = genres.First(g => g.Name == "Horror").Id
            },
            new()
            {
                Title = "The Matrix Resurrections",
                ReleaseDate = DateTime.Parse("2021-12-22"),
                Price = 11.99M,
                GenreId = genres.First(g => g.Name == "Thriller").Id
            },
            new()
            {
                Title = "Avatar: The Way of Water",
                ReleaseDate = DateTime.Parse("2022-12-16"),
                Price = 17.99M,
                GenreId = genres.First(g => g.Name == "Fantasy").Id
            }
        );

        context.SaveChanges();
    }
}
