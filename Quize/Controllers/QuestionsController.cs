using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quize.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Quize.Controllers
{
    /// <summary>
    /// Controller for managing Questions.
    /// </summary>
    [Authorize]
    public class QuestionsController : Controller
    {
        private readonly QuizDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// Initializes a new instance of the QuestionsController.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="webHostEnvironment">The web host environment.</param>
        public QuestionsController(QuizDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Displays the edit form for a specific question.
        /// </summary>
        /// <param name="id">The ID of the question to edit.</param>
        /// <returns>A view containing the edit form for the specified question.</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Answers_List)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        /// <summary>
        /// Processes the edit form submission for a question.
        /// </summary>
        /// <param name="id">The ID of the question being edited.</param>
        /// <param name="question">The updated question data.</param>
        /// <param name="splashImageFile">The new splash image file, if any.</param>
        /// <param name="answerTexts">The list of answer texts.</param>
        /// <param name="answerIds">The list of answer IDs.</param>
        /// <param name="CorrectAnswer">The ID of the correct answer.</param>
        /// <returns>Redirects to the Quiz Index action if successful, otherwise returns to the Edit view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,QuizId,CorrectAnswer")] Questions question, IFormFile? splashImageFile, List<string> answerTexts, List<int> answerIds, int CorrectAnswer)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Quiz");
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
                    question.Answers_List.Clear();
                    // Update answers
                    for (int i = 0; i < answerTexts.Count; i++)
                    {
                        Answers newAnswer = new Answers()
                        {
                            Id = answerIds[i],
                            Text = answerTexts[i],
                            QuestionId = question.Id,
                            CorrectAnswer = (answerIds[i] == CorrectAnswer)
                        };
                        question.Answers_List.Add(newAnswer);
                    }

                    // Handle splash image
                    if (splashImageFile != null)
                    {
                        question.SplashImage = await SaveImage(splashImageFile);
                    }
                    else
                    {
                        var existingQuestion = await _context.Questions.AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);
                        if (existingQuestion != null)
                        {
                            question.SplashImage = existingQuestion.SplashImage;
                        }
                    }

                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Quizzes", new { id = question.QuizId });
            }
            return View(question);
        }

        /// <summary>
        /// Checks if a question with the specified ID exists.
        /// </summary>
        /// <param name="id">The ID of the question to check.</param>
        /// <returns>True if the question exists, false otherwise.</returns>
        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }

        /// <summary>
        /// Displays the create form for a new question.
        /// </summary>
        /// <param name="QuizId">The ID of the quiz to which the question will be added.</param>
        /// <returns>A view containing the create form for a new question.</returns>
        [HttpGet("Questions/Create/{QuizId}")]
        public async Task<IActionResult> Create(int QuizId)
        {
            var question = new Questions { QuizId = QuizId };
            // Initialize with 4 empty answers
            for (int i = 0; i < 4; i++)
            {
                question.Answers_List.Add(new Answers());
            }
            return View(question);
        }

        /// <summary>
        /// Processes the create form submission for a new question.
        /// </summary>
        /// <param name="id">The ID of the quiz to which the question is being added.</param>
        /// <param name="question">The new question data.</param>
        /// <param name="splashImageFile">The splash image file for the question, if any.</param>
        /// <param name="answerTexts">The list of answer texts.</param>
        /// <param name="CorrectAnswer">The index of the correct answer.</param>
        /// <returns>Redirects to the Quiz Index action if successful, otherwise returns to the Create view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, [Bind("Text,QuizId,SplashImage")] Questions question, IFormFile? splashImageFile, List<string> answerTexts, int CorrectAnswer)
        {
            if (id != question.QuizId)
            {
                return NotFound();
            }

            ModelState.Remove("Quiz");
            if (ModelState.IsValid)
            {
                try
                {
                    // Handle splash image
                    if (splashImageFile != null)
                    {
                        question.SplashImage = await SaveImage(splashImageFile);
                    }

                    // Add answers
                    for (int i = 0; i < answerTexts.Count; i++)
                    {
                        var newAnswer = new Answers
                        {
                            Text = answerTexts[i],
                            QuestionId = question.Id,
                            CorrectAnswer = (i == CorrectAnswer)
                        };
                        question.Answers_List.Add(newAnswer);
                    }

                    _context.Add(question);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Quizzes", new { id = question.QuizId });
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(question);
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
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "questions");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}

