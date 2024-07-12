using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quize.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Quize.Controllers
{
    /// <summary>
    /// Controller for managing Tags.
    /// </summary>
    public class TagsController : Controller
    {
        private readonly QuizDbContext _context;

        /// <summary>
        /// Initializes a new instance of the TagsController.
        /// </summary>
        /// <param name="context">The database context.</param>
        public TagsController(QuizDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a list of all tags.
        /// </summary>
        /// <returns>A view containing a list of all tags.</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tags.ToListAsync());
        }

        /// <summary>
        /// Displays details of a specific tag, including associated quizzes.
        /// </summary>
        /// <param name="id">The ID of the tag to display.</param>
        /// <returns>A view containing the details of the specified tag.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags
                .Include(t => t.QuizzesTags_List)
                .ThenInclude(qt => qt.Quiz)
                .ThenInclude(a => a.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        /// <summary>
        /// Displays the edit form for a specific tag.
        /// </summary>
        /// <param name="id">The ID of the tag to edit.</param>
        /// <returns>A view containing the edit form for the specified tag.</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        /// <summary>
        /// Processes the edit form submission for a tag.
        /// </summary>
        /// <param name="id">The ID of the tag being edited.</param>
        /// <param name="tag">The updated tag data.</param>
        /// <returns>Redirects to the Index action if successful, otherwise returns to the Edit view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Tags tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagExists(tag.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        /// <summary>
        /// Checks if a tag with the specified ID exists.
        /// </summary>
        /// <param name="id">The ID of the tag to check.</param>
        /// <returns>True if the tag exists, false otherwise.</returns>
        private bool TagExists(int id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }

        /// <summary>
        /// Displays the create form for a new tag.
        /// </summary>
        /// <returns>A view containing the create form for a new tag.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Processes the create form submission for a new tag.
        /// </summary>
        /// <param name="tag">The new tag data.</param>
        /// <returns>Redirects to the Index action if successful, otherwise returns to the Create view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Tags tag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        /// <summary>
        /// Displays the delete confirmation page for a tag.
        /// </summary>
        /// <param name="id">The ID of the tag to delete.</param>
        /// <returns>A view asking for confirmation to delete the specified tag.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        /// <summary>
        /// Processes the tag deletion after confirmation.
        /// </summary>
        /// <param name="id">The ID of the tag to delete.</param>
        /// <returns>Redirects to the Index action after successful deletion.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
