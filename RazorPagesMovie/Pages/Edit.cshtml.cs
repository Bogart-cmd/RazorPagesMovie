using System;
using System.Collections.Generic;
using System.IO;                    // For file handling
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;     // For IFormFile
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages
{
    public class EditModel : PageModel
    {
        private readonly RazorPagesMovieContext _context;
        private readonly IWebHostEnvironment _environment;    // To access wwwroot

        public EditModel(RazorPagesMovieContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;   // Inject environment
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;

        [BindProperty]
        public List<SelectListItem> Genres { get; set; } = default!;

        [BindProperty]
        public IFormFile? ThumbnailImage { get; set; }  // For image uploads

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _context.Movie
                .Include(m => m.GenreRef)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Movie == null)
            {
                return NotFound();
            }

            // ✅ Ensure placeholder on GET if no thumbnail exists
            if (string.IsNullOrEmpty(Movie.ThumbnailUrl))
            {
                Movie.ThumbnailUrl = "/images/movies/placeholder.png";
            }

            Genres = await _context.Genre
                .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reload genres if validation fails
                Genres = await _context.Genre
                    .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
                    .ToListAsync();

                return Page();
            }

            // ✅ Fallback handling during POST
            if (ThumbnailImage != null && ThumbnailImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images/movies");
                var uniqueFileName = $"{Guid.NewGuid()}_{ThumbnailImage.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the uploaded image
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ThumbnailImage.CopyToAsync(fileStream);
                }

                // Set the new thumbnail URL
                Movie.ThumbnailUrl = $"/images/movies/{uniqueFileName}";
            }
            else
            {
                // ✅ If no new image, retain the current thumbnail or use placeholder
                Movie.ThumbnailUrl ??= "/images/movies/placeholder.png";
            }

            // Attach and update the movie
            _context.Attach(Movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(Movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
