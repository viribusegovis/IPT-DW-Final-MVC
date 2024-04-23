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


    public string SplashImage { get; set; }

    // Navigation properties

    public ICollection<Question> Questions { get; set; }
    public ICollection<Tag> TagList { get; set; }

    [ForeignKey(nameof(User))]
    public int AuthorId { get; set; }
    public User Author { get; set; }

    
}