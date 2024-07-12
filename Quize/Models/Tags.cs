using System.ComponentModel.DataAnnotations;
namespace Quize.Models
{
    // Tag model
    public class Tags
    {
        public Tags()
        {
            QuizzesTags_List = new HashSet<QuizzesTags>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Navigation property
        public ICollection<QuizzesTags> QuizzesTags_List { get; set; }
    }
}