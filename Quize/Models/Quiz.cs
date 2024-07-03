using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

// Quiz model
public class Quiz
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    public int AuthorId { get; set; }

    public string SplashImage { get; set; }

    // Navigation properties
    [ForeignKey("AuthorId")]
    public virtual Member Author { get; set; }

    public virtual ICollection<Question> Questions { get; set; }
}