// Answer model
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Answer
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Text { get; set; }

    public int QuestionId { get; set; }

    // Navigation property
    [ForeignKey("QuestionId")]
    public virtual Question Question { get; set; }
}