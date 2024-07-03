using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

// Question model
public class Question
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Text { get; set; }

    public int QuizId { get; set; }

    public string SplashImage { get; set; }

    public string CorrectAnswer { get; set; }

    // Navigation properties
    [ForeignKey("QuizId")]
    public virtual Quiz Quiz { get; set; }

    public virtual ICollection<Answer> Answers { get; set; }

    public virtual ICollection<QuestionTag> QuestionTags { get; set; }
}