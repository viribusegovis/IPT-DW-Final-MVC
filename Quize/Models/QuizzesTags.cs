using Azure;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace Quize.Models
{
    // QuizzesTags model
    [PrimaryKey(nameof(Quiz_Id), nameof(Tag_Id))]
    public class QuizzesTags
    {

        // Quizzes Foreign Key
        [ForeignKey(nameof(Quiz))]
        public int Quiz_Id { get; set; }
        public Quizzes Quiz { get; set; }

        // Tag Foreign Key
        [ForeignKey(nameof(Tag))]
        public int Tag_Id { get; set; }
        public Tags Tag { get; set; }
    }
}