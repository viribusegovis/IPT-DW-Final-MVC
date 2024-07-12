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
    [Authorize]
    public class QuestionsController : Controller
    {
        private readonly QuizDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuestionsController(QuizDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Questions/Edit/5
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

        // POST: Questions/Edit/5
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

            if (ModelState.IsValid){
            
                try {
                    question.Answers_List.Clear();
                    // Remove existing answers
                    for (int i =0; i<answerTexts.Count; i++) {
                        Answers newAnswer;
                        if (answerIds[i] == CorrectAnswer)
                        {
                             newAnswer= new Answers() { Id = answerIds[i], Text = answerTexts[i], QuestionId = question.Id, CorrectAnswer = true };
                        }
                        else
                        {
                             newAnswer = new Answers() { Id = answerIds[i], Text = answerTexts[i], QuestionId = question.Id, CorrectAnswer = false };
                        }

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

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }

        // GET: Questions/Create
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

        // POST: Questions/Create
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
                        if (i == CorrectAnswer)
                        {
                            var newAnswer = new Answers { Text = answerTexts[i], QuestionId = question.Id, CorrectAnswer = true };
                            question.Answers_List.Add(newAnswer);
                        }
                        else
                        {
                            var newAnswer = new Answers { Text = answerTexts[i], QuestionId = question.Id, CorrectAnswer = false };
                            question.Answers_List.Add(newAnswer);
                        }
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
