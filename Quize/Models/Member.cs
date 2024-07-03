using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// User model
public class Member
{
    [Key]
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Username { get; set; }

    // Navigation property
    public virtual ICollection<Quiz> Quizzes { get; set; }
}