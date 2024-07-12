using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Quiz.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Custom: Add QuizDbContext as the application's DbContext
builder.Services.AddDbContext<QuizDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Custom: Configure Identity with additional options
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Add role support
    .AddEntityFrameworkStores<QuizDbContext>(); // Use QuizDbContext for Identity storage
builder.Services.AddControllersWithViews();

// Custom: Register custom EmailSender service
builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Static files middleware
// Serves files from the wwwroot folder without authentication by default
app.UseStaticFiles();

app.UseRouting();

// Authentication and Authorization middleware
// Placed after UseRouting but before UseEndpoints to apply to all routes
app.UseAuthentication();
app.UseAuthorization();

// Custom: Redirect root URL to Dashboard
app.MapGet("/", context => {
    context.Response.Redirect("/Dashboard");
    return Task.CompletedTask;
});

// Custom: Set default route to use Dashboard controller instead of Home
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
