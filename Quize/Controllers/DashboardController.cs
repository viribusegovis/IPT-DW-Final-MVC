using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Quize.Models;

namespace Quize.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public IActionResult Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalQuizzes = 150,
                ActiveUsers = 1234,
                QuizzesTakenToday = 89,
                AvgCompletionRate = 76.5,
                TopQuizzes = new List<QuizViewModel>
                {
                    new QuizViewModel { Title = "General Knowledge", CompletionCount = 245 },
                    new QuizViewModel { Title = "Science Quiz", CompletionCount = 198 },
                    new QuizViewModel { Title = "History Trivia", CompletionCount = 176 },
                    new QuizViewModel { Title = "Pop Culture", CompletionCount = 154 },
                    new QuizViewModel { Title = "Math Challenge", CompletionCount = 132 }
                },
                RecentUsers = new List<DashboardUserViewModel>
                {
                    new DashboardUserViewModel { Name = "John Doe", Email = "john@example.com" },
                    new DashboardUserViewModel { Name = "Jane Smith", Email = "jane@example.com" },
                    new DashboardUserViewModel { Name = "Bob Johnson", Email = "bob@example.com" },
                    new DashboardUserViewModel { Name = "Alice Brown", Email = "alice@example.com" },
                    new DashboardUserViewModel { Name = "Charlie Davis", Email = "charlie@example.com" }
                },
                ChartLabels = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun" },
                ChartData = new List<int> { 65, 59, 80, 81, 56, 55 }
            };

            return View(viewModel);
        }
    }
}
