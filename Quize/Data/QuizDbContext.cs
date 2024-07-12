using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quize.Models;
using System;
using System.Configuration;

/// <summary>
/// Represents the database context for the Quiz application, extending IdentityDbContext for user management.
/// </summary>
public class QuizDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the QuizDbContext.
    /// </summary>
    /// <param name="options">The options to be used by a DbContext.</param>
    /// <param name="configuration">The configuration to be used for seeding default data.</param>
    public QuizDbContext(DbContextOptions<QuizDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Gets or sets the Members DbSet.
    /// </summary>
    public DbSet<Members> Members { get; set; }

    /// <summary>
    /// Gets or sets the Quizzes DbSet.
    /// </summary>
    public DbSet<Quizzes> Quizzes { get; set; }

    /// <summary>
    /// Gets or sets the Questions DbSet.
    /// </summary>
    public DbSet<Questions> Questions { get; set; }

    /// <summary>
    /// Gets or sets the Answers DbSet.
    /// </summary>
    public DbSet<Answers> Answers { get; set; }

    /// <summary>
    /// Gets or sets the Tags DbSet.
    /// </summary>
    public DbSet<Tags> Tags { get; set; }

    /// <summary>
    /// Gets or sets the QuizzesTags DbSet.
    /// </summary>
    public DbSet<QuizzesTags> QuizzesTags { get; set; }

    /// <summary>
    /// Configures the schema needed for the identity framework and seeds default data.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Create default admin user
        var defaultUser = new IdentityUser
        {
            UserName = _configuration["DefaultIdentityUser:UserName"],
            NormalizedUserName = _configuration["DefaultIdentityUser:UserName"]!.ToUpper(),
            Email = _configuration["DefaultIdentityUser:UserEmail"],
            NormalizedEmail = _configuration["DefaultIdentityUser:UserEmail"]!.ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        // Hash the password
        var passwordHasher = new PasswordHasher<IdentityUser>();
        defaultUser.PasswordHash = passwordHasher.HashPassword(defaultUser, _configuration["DefaultIdentityUser:UserPassword"]!);

        modelBuilder.Entity<IdentityUser>().HasData(defaultUser);

        // Create default admin role
        var adminRole = new IdentityRole
        {
            Name = "Admin",
            NormalizedName = "ADMIN"
        };

        modelBuilder.Entity<IdentityRole>().HasData(adminRole);

        // Add user to admin role
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = adminRole.Id,
            UserId = defaultUser.Id
        });
    }
}
