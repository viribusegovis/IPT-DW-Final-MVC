namespace Quize.Models
{
    public class DashboardViewModel
    {
        public int TotalQuizzes { get; set; }
        public int ActiveUsers { get; set; }
        public int QuizzesTakenToday { get; set; }
        public double AvgCompletionRate { get; set; }
        public List<QuizViewModel> TopQuizzes { get; set; }
        public List<DashboardUserViewModel> RecentUsers { get; set; }
        public List<string> ChartLabels { get; set; }
        public List<int> ChartData { get; set; }
    }

    public class QuizViewModel
    {
        public string Title { get; set; }
        public int CompletionCount { get; set; }
    }

    public class DashboardUserViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}