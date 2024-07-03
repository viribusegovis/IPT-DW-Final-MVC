using Microsoft.EntityFrameworkCore;

public class QuizDbContext : DbContext
{
    public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
    {
    }

    public DbSet<Member> Members { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<QuestionTag> QuestionTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuestionTag>()
            .HasKey(qt => new { qt.QuestionId, qt.TagId });
    }
}