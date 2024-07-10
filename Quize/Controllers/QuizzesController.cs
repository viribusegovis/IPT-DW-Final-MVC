using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quize.Helpers;
using Quize.Models;
using System.Drawing.Printing;
using System.IO;

namespace Quize.Controllers
{
    [Authorize]
    public class QuizzesController : Controller
    {
        private readonly QuizDbContext _context;
        private readonly int PageSize = 1;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuizzesController(QuizDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

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


        // GET: Quizzes/Edit/
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





        // POST: Quizzes/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,AuthorId")] Quizzes quiz,List<int> selectedTags, IFormFile splashImageFile)
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


        private async Task<string> SaveImage(IFormFile imageFile)
        {
            string uniqueFileName = null;

            if (imageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
            }

            return uniqueFileName;
        }

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

        private bool QuizExists(int id)
        {
            return _context.Quizzes.Any(e => e.Id == id);
        }
    }
}