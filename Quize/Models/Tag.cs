using System.ComponentModel.DataAnnotations;


// Tag model
public class Tag
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    // Navigation property
    public virtual ICollection<QuestionTag> QuestionTags { get; set; }
}