using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

// Question model
public class Question
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Text { get; set; }


    public string SplashImage { get; set; }

    public string CorrectAnswer { get; set; }

    
    [ForeignKey(nameof(Quiz))] 
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; }

    public ICollection<Answer> Answers { get; set; }


}