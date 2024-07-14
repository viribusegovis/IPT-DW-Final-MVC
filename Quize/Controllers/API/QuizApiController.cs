using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quize.Models;
using System;
using System.Threading.Tasks;

namespace Quize.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizApiController : ControllerBase
    {
        private readonly QuizDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public QuizApiController(QuizDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }

        /// <summary>
        /// Retrieves all quizzes with their associated questions and answers.
        /// </summary>
        /// <returns>A list of all quizzes with their questions and answers.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllQuizzes([FromQuery] int? authorId)
        {
            var quizzesQuery = _context.Quizzes
                .Include(q => q.Author)
                .Include(q => q.QuizzesTags_List)
                    .ThenInclude(qt => qt.Tag)
                .Include(q => q.Questions_List)
                    .ThenInclude(q => q.Answers_List)
                .AsQueryable();

            if (authorId.HasValue)
            {
                quizzesQuery = quizzesQuery.Where(q => q.Author.Id == authorId.Value);
            }

            var quizzes = await quizzesQuery
                .Select(q => new
                {
                    q.Id,
                    q.Title,
                    q.Description,
                    q.SplashImage,
                    Author = q.Author.Username,
                    AuthorId = q.Author.Id,
                    Tags = q.QuizzesTags_List.Select(qt => qt.Tag.Name),
                    Questions = q.Questions_List.Select(ques => new
                    {
                        ques.Id,
                        ques.Text,
                        ques.SplashImage,
                        Answers = ques.Answers_List.Select(ans => new
                        {
                            ans.Id,
                            ans.Text,
                            ans.CorrectAnswer
                        })
                    })
                })
                .ToListAsync();

            return Ok(quizzes);
        }


        public async Task<IActionResult> GetQuiz(int? quizId)
        {
            var quizzesQuery = _context.Quizzes
                .Include(q => q.Author)
                .Include(q => q.QuizzesTags_List)
                    .ThenInclude(qt => qt.Tag)
                .Include(q => q.Questions_List)
                    .ThenInclude(q => q.Answers_List)
                .AsQueryable();

            if (quizId.HasValue)
            {
                quizzesQuery = quizzesQuery.Where(q => q.Id == quizId.Value);
            }

            var quizzes = await quizzesQuery
                .Select(q => new
                {
                    q.Id,
                    q.Title,
                    q.Description,
                    q.SplashImage,
                    Author = q.Author.Username,
                    AuthorId = q.Author.Id,
                    Tags = q.QuizzesTags_List.Select(qt => qt.Tag.Name),
                    Questions = q.Questions_List.Select(ques => new
                    {
                        ques.Id,
                        ques.Text,
                        ques.SplashImage,
                        Answers = ques.Answers_List.Select(ans => new
                        {
                            ans.Id,
                            ans.Text,
                            ans.CorrectAnswer
                        })
                    })
                })
                .ToListAsync();

            return Ok(quizzes);
        }

        /// <summary>
        /// Deletes a quiz and all its related data (questions, answers, and quiz-tag relationships), but not the tags themselves.
        /// </summary>
        /// <param name="id">The ID of the quiz to delete.</param>
        /// <returns>NoContent if successful, NotFound if the quiz doesn't exist, or BadRequest if there's an error.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            try
            {
                var quiz = await _context.Quizzes
                    .Include(q => q.Questions_List)
                        .ThenInclude(q => q.Answers_List)
                    .Include(q => q.QuizzesTags_List)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (quiz == null)
                {
                    return NotFound($"Quiz with ID {id} not found.");
                }

                // Remove QuizzesTags relationships
                _context.QuizzesTags.RemoveRange(quiz.QuizzesTags_List);

                // Remove Answers and Questions
                foreach (var question in quiz.Questions_List)
                {
                    _context.Answers.RemoveRange(question.Answers_List);
                }
                _context.Questions.RemoveRange(quiz.Questions_List);

                // Remove the Quiz
                _context.Quizzes.Remove(quiz);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error deleting quiz: {ex.Message}");
                return BadRequest($"Error deleting quiz: {ex.Message}");
            }


        }

        /// <summary>
        /// Edits an existing quiz, including its questions and answers.
        /// </summary>
        /// <param name="id">The ID of the quiz to edit.</param>
        /// <param name="quizDto">The updated quiz data, including questions and answers.</param>
        /// <returns>The updated quiz if successful, BadRequest if there's an error.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditQuiz(int id, [FromForm] QuizUploadModel model)
        {
            if (model?.quizJson == null)
            {
                return BadRequest("Quiz data is required");
            }

            var quizDto = JsonConvert.DeserializeObject<QuizDto>(model.quizJson);

            if (id != quizDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            var quiz = await _context.Quizzes
                .Include(q => q.Questions_List)
                    .ThenInclude(q => q.Answers_List)
                .Include(q => q.QuizzesTags_List)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
            {
                return NotFound($"Quiz with ID {id} not found.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Update basic quiz properties
                quiz.Title = quizDto.Title;
                quiz.Description = quizDto.Description;
                quiz.AuthorId = int.Parse(quizDto.AuthorId);

                // Handle quiz image upload
                if (model.quizImage != null)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_"+DateTime.UtcNow+ "_" + Path.GetExtension(model.quizImage.FileName);
                    var uploads = Path.Combine(_environment.WebRootPath, "uploads/quizzes");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    Directory.CreateDirectory(uploads);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.quizImage.CopyToAsync(fileStream);
                    }
                    quiz.SplashImage = uniqueFileName;
                }

                // Update questions and answers
                UpdateQuestionsAndAnswers(quiz, quizDto.Questions.Select(q => new Questions
                {
                    Id = q.Id ?? 0,
                    Text = q.Text,
                    Answers_List = q.Answers.Select(a => new Answers
                    {
                        Id = a.Id,
                        Text = a.Text,
                        CorrectAnswer = a.CorrectAnswer
                    }).ToList()
                }).ToList());

                // Update tags

                // Handle question images
                if (model.questionImages != null && model.questionImages.Any())
                {
                    foreach (var questionImage in model.questionImages)
                    {
                        
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + DateTime.UtcNow.ToFileTimeUtc() + questionImage.FileName;
                        var uploads = Path.Combine(_environment.WebRootPath, "uploads/questions");
                        var filePath = Path.Combine(uploads, uniqueFileName);
                        Directory.CreateDirectory(uploads);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await questionImage.CopyToAsync(fileStream);
                        }
                        var questionIdString = Path.GetFileNameWithoutExtension(questionImage.FileName).Split('_')[0];
                        if (int.TryParse(questionIdString, out int questionId))
                        {
                            var question = quiz.Questions_List.FirstOrDefault(q => q.Id == questionId);
                            if (question != null)
                            {
                                question.SplashImage = uniqueFileName;
                            }
                        }
                        else
                        {
                            // Log an error or handle the case where the filename doesn't contain a valid question ID
                            Console.WriteLine($"Invalid question ID in filename: {questionImage.FileName}");
                        }


                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Prepare DTO for response
                var updatedQuizDto = new QuizDto
                {
                    Id = quiz.Id,
                    Title = quiz.Title,
                    Description = quiz.Description,
                    AuthorId = quiz.AuthorId.ToString(),
                    Questions = quiz.Questions_List.Select(q => new QuestionDto
                    {
                        Id = q.Id,
                        Text = q.Text,
                        Answers = q.Answers_List.Select(a => new AnswerDto
                        {
                            Id = a.Id,
                            Text = a.Text,
                            CorrectAnswer = a.CorrectAnswer
                        }).ToList()
                    }).ToList(),
                };

                return Ok(updatedQuizDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Error updating quiz: {ex.Message}");
            }
        }


        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString()
                   + Path.GetExtension(fileName);
        }

        private void UpdateQuizTags(Quizzes quiz, List<int> newTagIds)
        {
            var currentTagIds = quiz.QuizzesTags_List.Select(qt => qt.Tag_Id).ToList();
            var tagsToRemove = currentTagIds.Except(newTagIds).ToList();
            var tagsToAdd = newTagIds.Except(currentTagIds).ToList();

            foreach (var tagId in tagsToRemove)
            {
                var quizTag = quiz.QuizzesTags_List.FirstOrDefault(qt => qt.Tag_Id == tagId);
                if (quizTag != null)
                {
                    quiz.QuizzesTags_List.Remove(quizTag);
                }
            }

            foreach (var tagId in tagsToAdd)
            {
                quiz.QuizzesTags_List.Add(new QuizzesTags { Quiz_Id = quiz.Id, Tag_Id = tagId });
            }
        }

        private void UpdateQuestionsAndAnswers(Quizzes quiz, List<Questions> newQuestions)
        {
            var currentQuestionIds = quiz.Questions_List.Select(q => q.Id).ToList();
            var questionsToRemove = currentQuestionIds.Except(newQuestions.Select(q => q.Id)).ToList();

            // Remove questions that are not in the new list
            foreach (var questionId in questionsToRemove)
            {
                var questionToRemove = quiz.Questions_List.FirstOrDefault(q => q.Id == questionId);
                if (questionToRemove != null)
                {
                    _context.Answers.RemoveRange(questionToRemove.Answers_List);
                    quiz.Questions_List.Remove(questionToRemove);
                }
            }

            // Update or add questions
            foreach (var newQuestion in newQuestions)
            {
                var existingQuestion = quiz.Questions_List.FirstOrDefault(q => q.Id == newQuestion.Id);
                if (existingQuestion != null)
                {
                    // Update existing question
                    existingQuestion.Text = newQuestion.Text;

                    UpdateAnswers(existingQuestion, newQuestion.Answers_List.ToList());
                }
                else
                {
                    // Add new question
                    var question = new Questions
                    {
                        Text = newQuestion.Text,
                        SplashImage = newQuestion.SplashImage,
                        Answers_List = newQuestion.Answers_List.Select(a => new Answers
                        {
                            Text = a.Text,
                            CorrectAnswer = a.CorrectAnswer
                        }).ToList()
                    };
                    quiz.Questions_List.Add(question);
                }
            }
        }

        private void UpdateAnswers(Questions question, List<Answers> newAnswers)
        {
            var currentAnswerIds = question.Answers_List.Select(a => a.Id).ToList();
            var answersToRemove = currentAnswerIds.Except(newAnswers.Select(a => a.Id)).ToList();

            // Remove answers that are not in the new list
            foreach (var answerId in answersToRemove)
            {
                var answerToRemove = question.Answers_List.FirstOrDefault(a => a.Id == answerId);
                if (answerToRemove != null)
                {
                    question.Answers_List.Remove(answerToRemove);
                }
            }

            // Update or add answers
            foreach (var newAnswer in newAnswers)
            {
                var existingAnswer = question.Answers_List.FirstOrDefault(a => a.Id == newAnswer.Id);
                if (existingAnswer != null)
                {
                    // Update existing answer
                    existingAnswer.Text = newAnswer.Text;
                    existingAnswer.CorrectAnswer = newAnswer.CorrectAnswer;
                }
                else
                {
                    // Add new answer
                    question.Answers_List.Add(new Answers
                    {
                        Text = newAnswer.Text,
                        CorrectAnswer = newAnswer.CorrectAnswer
                    });
                }
            }

            

        }
        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromForm] QuizUploadModel model)
        {
            if (model?.quizJson == null)
            {
                return BadRequest("Quiz data is required");
            }

            var quizDto = JsonConvert.DeserializeObject<QuizDto>(model.quizJson);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var quiz = new Quizzes
                {
                    Title = quizDto.Title,
                    Description = quizDto.Description,
                    AuthorId = int.Parse(quizDto.AuthorId)
                };

                // Handle quiz image upload
                if (model.quizImage != null)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + DateTime.UtcNow.ToFileTimeUtc() + "." + Path.GetExtension(model.quizImage.FileName);
                    var uploads = Path.Combine(_environment.WebRootPath, "uploads/quizzes");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    Directory.CreateDirectory(uploads);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.quizImage.CopyToAsync(fileStream);
                    }
                    quiz.SplashImage = uniqueFileName;
                }

                // Add questions and answers
                quiz.Questions_List = quizDto.Questions.Select(q => new Questions
                {
                    Text = q.Text,
                    Answers_List = q.Answers.Select(a => new Answers
                    {
                        Text = a.Text,
                        CorrectAnswer = a.CorrectAnswer
                    }).ToList()
                }).ToList();

                _context.Quizzes.Add(quiz);
                await _context.SaveChangesAsync();

                // Handle question images
                if (model.questionImages != null && model.questionImages.Any())
                {
                    foreach (var questionImage in model.questionImages)
                    {
                        var questionIdString = Path.GetFileNameWithoutExtension(questionImage.FileName).Split('_')[0];
                        if (int.TryParse(questionIdString, out int questionId))
                        {
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + DateTime.UtcNow.ToFileTimeUtc() + "." + Path.GetExtension(questionImage.FileName);
                            var uploads = Path.Combine(_environment.WebRootPath, "uploads/questions");
                            var filePath = Path.Combine(uploads, uniqueFileName);
                            Directory.CreateDirectory(uploads);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await questionImage.CopyToAsync(fileStream);
                            }

                            var question = quiz.Questions_List.FirstOrDefault(q => q.Id == questionId);
                            if (question != null)
                            {
                                question.SplashImage = uniqueFileName;
                            }
                        }
                        else
                        {
                            // Log an error or handle the case where the filename doesn't contain a valid question ID
                            Console.WriteLine($"Invalid question ID in filename: {questionImage.FileName}");
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Prepare DTO for response
                var createdQuizDto = new QuizDto
                {
                    Id = quiz.Id,
                    Title = quiz.Title,
                    Description = quiz.Description,
                    AuthorId = quiz.AuthorId.ToString(),
                    Questions = quiz.Questions_List.Select(q => new QuestionDto
                    {
                        Id = q.Id,
                        Text = q.Text,
                        Answers = q.Answers_List.Select(a => new AnswerDto
                        {
                            Id = a.Id,
                            Text = a.Text,
                            CorrectAnswer = a.CorrectAnswer
                        }).ToList()
                    }).ToList(),
                };

                return CreatedAtAction(nameof(GetAllQuizzes), new { id = quiz.Id }, createdQuizDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Error creating quiz: {ex.Message}");
            }
        }

    }

    public class QuizDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? AuthorId { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }

    public class QuestionDto
    {
        public int? Id { get; set; }
        public string Text { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }

    public class AnswerDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool CorrectAnswer { get; set; }
    }
    public class QuizUploadModel
    {
        public string? quizJson { get; set; }
        public IFormFile? quizImage { get; set; }
        public List<IFormFile>? questionImages { get; set; }
    }



}
