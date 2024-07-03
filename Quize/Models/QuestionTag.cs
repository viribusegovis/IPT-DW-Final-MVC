using Azure;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

// QuestionTag model
public class QuestionTag
{
    [Key]
    [Column(Order = 0)]
    public int QuestionId { get; set; }

    [Key]
    [Column(Order = 1)]
    public int TagId { get; set; }

    // Navigation properties
    [ForeignKey("QuestionId")]
    public virtual Question Question { get; set; }

    [ForeignKey("TagId")]
    public virtual Tag Tag { get; set; }
}