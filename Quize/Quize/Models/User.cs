using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// User model
public class User
{
    [Key]
    public int Id { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string Password { get; set; }

    public string Username { get; set; }

    // Navigation property
    public ICollection<Quiz> Quizzes { get; set; }
}