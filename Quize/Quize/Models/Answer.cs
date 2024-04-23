// Answer model
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Answer
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Text { get; set; }


    // Navigation property
    [ForeignKey(nameof(Question))]
    public int QuestionId { get; set; }
    public Question Question { get; set; }
}