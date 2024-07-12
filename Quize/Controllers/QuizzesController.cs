using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quize.Helpers;
using Quize.Models;
using System.IO;

namespace Quize.Controllers
{
    /// <summary>
    /// Controller for managing Quizzes.
    /// </summary>
    [Authorize]
    public class QuizzesController : Controller
    {
        private readonly QuizDbContext _context;
        private readonly int PageSize = 1;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// Initializes a new instance of the QuizzesController.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="webHostEnvironment">The web host environment.</param>
        public QuizzesController(QuizDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Displays a paginated list of quizzes, with optional search functionality.
        /// </summary>
        /// <param name="searchTerm">The search term to filter quizzes.</param>
        /// <param name="pageNumber">The page number to display.</param>
        /// <returns>A view containing a paginated list of quizzes.</returns>
        public async Task<IActionResult> Index(string searchTerm, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchTerm;

            var quizzes = from q in _context.Quizzes
                          .Include(q => q.Questions_List)
                          .ThenInclude(q => q.Answers_List)
                          .Include(q => q.QuizzesTags_List)
                          .ThenInclude(qt => qt.Tag)
                          .Include(q => q.Author)
                          select q;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                quizzes = quizzes.Where(q => q.Title.Contains(searchTerm));
            }

            int pageNum = pageNumber ?? 1;
            var paginatedQuizzes = await PaginatedList<Quizzes>.CreateAsync(quizzes.AsNoTracking(), pageNum, PageSize);

            return View(paginatedQuizzes);
        }

        /// <summary>
        /// Displays the edit form for a specific quiz.
        /// </summary>
        /// <param name="id">The ID of the quiz to edit.</param>
        /// <returns>A view containing the edit form for the specified quiz.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Author)
                .Include(q => q.QuizzesTags_List)
                .ThenInclude(qt => qt.Tag)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
            {
                return NotFound();
            }

            var selectedTagIds = quiz.QuizzesTags_List.Select(qt => qt.Tag.Id).ToList();
            ViewBag.AvailableTags = new MultiSelectList(_context.Tags, "Id", "Name", selectedTagIds);
            ViewBag.Authors = new SelectList(_context.Members, "Id", "Username", quiz.Author.Id);

            return View(quiz);
        }

        /// <summary>
        /// Processes the edit form submission for a quiz.
        /// </summary>
        /// <param name="id">The ID of the quiz being edited.</param>
        /// <param name="quiz">The updated quiz data.</param>
        /// <param name="selectedTags">The list of selected tag IDs.</param>
        /// <param name="splashImageFile">The new splash image file, if any.</param>
        /// <returns>Redirects to the Index action if successful, otherwise returns to the Edit view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,AuthorId")] Quizzes quiz, List<int> selectedTags, IFormFile splashImageFile)
        {
            if (id != quiz.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Author");
            if (!ModelState.IsValid)
            {
                // Debug: Print ModelState errors
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
                    var quizToUpdate = await _context.Quizzes
                        .Include(q => q.QuizzesTags_List)
                        .FirstOrDefaultAsync(q => q.Id == id);

                    if (quizToUpdate == null)
                    {
                        return NotFound();
                    }

                    // Update properties
                    quizToUpdate.Title = quiz.Title;
                    quizToUpdate.Description = quiz.Description;
                    quizToUpdate.AuthorId = quiz.AuthorId;

                    // Handle splash image
                    if (splashImageFile != null)
                    {
                        quizToUpdate.SplashImage = await SaveImage(splashImageFile);
                    }

                    // Update tags
                    UpdateTags(quizToUpdate, selectedTags);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizExists(quiz.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.AvailableTags = new MultiSelectList(_context.Tags, "Id", "Name", selectedTags);
            ViewBag.Authors = new SelectList(_context.Members, "Id", "Username", quiz.AuthorId);
            return View(quiz);
        }

        /// <summary>
        /// Saves an image file to the server.
        /// </summary>
        /// <param name="imageFile">The image file to save.</param>
        /// <returns>The unique filename of the saved image.</returns>
        private async Task<string> SaveImage(IFormFile imageFile)
        {
            string uniqueFileName = null;

            if (imageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "quizzes");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
            }

            return uniqueFileName;
        }

        /// <summary>
        /// Updates the tags associated with a quiz.
        /// </summary>
        /// <param name="existingQuiz">The quiz to update.</param>
        /// <param name="selectedTags">The list of selected tag IDs.</param>
        private void UpdateTags(Quizzes existingQuiz, List<int> selectedTags)
        {
            var currentTags = existingQuiz.QuizzesTags_List.ToList();
            var tagsToRemove = currentTags.Where(t => !selectedTags.Contains(t.Tag_Id)).ToList();
            var tagsToAdd = selectedTags.Where(t => !currentTags.Any(ct => ct.Tag_Id == t)).ToList();

            foreach (var tagToRemove in tagsToRemove)
            {
                existingQuiz.QuizzesTags_List.Remove(tagToRemove);
            }

            foreach (var tagToAdd in tagsToAdd)
            {
                existingQuiz.QuizzesTags_List.Add(new QuizzesTags { Quiz_Id = existingQuiz.Id, Tag_Id = tagToAdd });
            }
        }

        /// <summary>
        /// Checks if a quiz with the specified ID exists.
        /// </summary>
        /// <param name="id">The ID of the quiz to check.</param>
        /// <returns>True if the quiz exists, false otherwise.</returns>
        private bool QuizExists(int id)
        {
            return _context.Quizzes.Any(e => e.Id == id);
        }
    }
}