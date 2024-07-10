using Quize.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quize.Models
{
    public class Quizzes
    {
        public Quizzes()
        {
            Questions_List = new HashSet<Questions>();
            QuizzesTags_List = new HashSet<QuizzesTags>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }

        public string? SplashImage { get; set; }

        public Members Author { get; set; }

        public ICollection<Questions> Questions_List { get; set; }
        public ICollection<QuizzesTags> QuizzesTags_List { get; set; }
    }
}