// Answer model
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Quize.Models
{
    public class Answers
    {
        public Answers()
        {

        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public int QuestionId { get; set; }

        // Navigation property
        [ForeignKey("QuestionId")]
        public Questions Question { get; set; }
    }
}