using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quize.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Quize.Controllers
{
    /// <summary>
    /// Controller for managing Members.
    /// </summary>
    [Authorize]
    public class MembersController : Controller
    {
        private readonly QuizDbContext _context;

        /// <summary>
        /// Initializes a new instance of the MembersController.
        /// </summary>
        /// <param name="context">The database context.</param>
        public MembersController(QuizDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a list of all members.
        /// </summary>
        /// <returns>A view containing a list of all members.</returns>
        public async Task<IActionResult> Index()
        {
            var members = await _context.Members.ToListAsync();
            return View(members);
        }

        /// <summary>
        /// Displays the edit form for a specific member.
        /// </summary>
        /// <param name="id">The ID of the member to edit.</param>
        /// <returns>A view containing the edit form for the specified member.</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        /// <summary>
        /// Processes the edit form submission for a member.
        /// </summary>
        /// <param name="id">The ID of the member being edited.</param>
        /// <param name="member">The updated member data.</param>
        /// <returns>Redirects to the Index action if successful, otherwise returns to the Edit view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Username")] Members member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            // Debug: Print ModelState errors
            if (!ModelState.IsValid)
            {
                foreach (var modelStateEntry in ModelState)
                {
                    var key = modelStateEntry.Key;
                    var errors = modelStateEntry.Value.Errors;
                    if (errors.Count > 0)
                    {
                        Console.WriteLine($"Invalid property: {key}");
                        foreach (var error in errors)
                        {
                            Console.WriteLine($"  Error: {error.ErrorMessage}");
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
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
            return View(member);
        }

        /// <summary>
        /// Displays details of a specific member, including their quizzes and questions.
        /// </summary>
        /// <param name="id">The ID of the member to display.</param>
        /// <returns>A view containing the details of the specified member.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Fetch the member with their quizzes and questions
            var member = await _context.Members
                .Include(m => m.Quizzes)
                    .ThenInclude(q => q.Questions_List)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
            {
                return NotFound();
            }

            // Explicitly load the Questions_List for each quiz
            foreach (var quiz in member.Quizzes)
            {
                await _context.Entry(quiz)
                    .Collection(q => q.Questions_List)
                    .LoadAsync();
            }

            return View(member);
        }

        /// <summary>
        /// Displays the delete confirmation page for a member.
        /// </summary>
        /// <param name="id">The ID of the member to delete.</param>
        /// <returns>A view asking for confirmation to delete the specified member.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        /// <summary>
        /// Processes the member deletion after confirmation.
        /// </summary>
        /// <param name="id">The ID of the member to delete.</param>
        /// <returns>Redirects to the Index action after successful deletion.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Checks if a member with the specified ID exists.
        /// </summary>
        /// <param name="id">The ID of the member to check.</param>
        /// <returns>True if the member exists, false otherwise.</returns>
        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
