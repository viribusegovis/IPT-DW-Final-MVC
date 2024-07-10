using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quize.Models;
using System;
using System.Configuration;

public class QuizDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IConfiguration _configuration;

    public QuizDbContext(DbContextOptions<QuizDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Members> Members { get; set; }
    public DbSet<Quizzes> Quizzes { get; set; }
    public DbSet<Questions> Questions { get; set; }
    public DbSet<Answers> Answers { get; set; }
    public DbSet<Tags> Tags { get; set; }
    public DbSet<QuizzesTags> QuizzesTags { get; set; }

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
