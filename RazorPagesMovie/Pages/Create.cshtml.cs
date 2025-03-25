using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPagesMovie.Models;
using System.IO;

namespace RazorPagesMovie.Pages;

public class CreateModel : PageModel
{
    private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;
    private readonly IWebHostEnvironment _environment;

    public CreateModel(RazorPagesMovie.Data.RazorPagesMovieContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [BindProperty]
    public Movie Movie { get; set; } = default!;

    public IActionResult OnGet()
    {
        ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "Name");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(IFormFile? ThumbnailFile)
    {
        if (!ModelState.IsValid)
        {
            ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "Name");
            return Page();
        }

        if (ThumbnailFile != null && ThumbnailFile.Length > 0)
        {
            // Create uploads folder if it doesn't exist
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images/movies");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique filename
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ThumbnailFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the image file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ThumbnailFile.CopyToAsync(fileStream);
            }

            // Store the relative URL in the database
            Movie.ThumbnailUrl = $"/images/movies/{uniqueFileName}";
        }

        _context.Movie.Add(Movie);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
