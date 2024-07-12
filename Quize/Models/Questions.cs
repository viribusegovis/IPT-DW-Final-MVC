using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Quize.Models
{
    // Question model
    public class Questions
    {

        public Questions()
        {
            Answers_List = new HashSet<Answers>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public int QuizId { get; set; }

        public string? SplashImage { get; set; }

        // Navigation properties
        [ForeignKey("QuizId")]
        public Quizzes Quiz { get; set; }

        public ICollection<Answers> Answers_List { get; set; }
    }
}