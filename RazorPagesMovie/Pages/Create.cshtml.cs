using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;
using Microsoft.EntityFrameworkCore;

namespace RazorPagesMovie.Pages
{
    public class CreateModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public CreateModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // Load genres into a dropdown list
            ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reload the genres in case of validation errors
                ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "Name");
                return Page();
            }

            // Attach the genre reference
            var genre = await _context.Genre.FindAsync(Movie.GenreId);
            if (genre != null)
            {
                Movie.GenreRef = genre;
            }

            _context.Movie.Add(Movie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
