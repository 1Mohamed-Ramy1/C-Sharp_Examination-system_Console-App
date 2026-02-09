using System.IO;
using System.Linq;
using MID_PROJ.Routes;
using MID_PROJ.Services;
using MID_PROJ.Models;
using MID_PROJ.Utils;

namespace MID_PROJ.Pages;
public class AdminPage : Page
{
    public override void Display()
    {
        var welcomeName = AppState.CurrentUser?.IsAdmin == true ? "ELOSTORA" : AppState.CurrentUser?.Username;
        if (!string.IsNullOrWhiteSpace(welcomeName))
        {
            Print.OutLine($"👋 Welcome {welcomeName}", ConsoleColor.Yellow);
        }
        Print.OutLine("�‍🏫 ═══════════ TEACHER PANEL ═══════════ 👨‍🏫",ConsoleColor.DarkCyan);
        Console.WriteLine(new string('═',60));
    }

    public override void HandleInput(Router router)
    {
        while(true)
        {
            Console.Clear();
            Display();
            Console.WriteLine();

            var choice= Print.AskChoice("Select an option", new List<string>
            {
                "👥 View All Users",
                "📝 View All Exams",
                "📊 View All Results",
                "📈 View Statistics",
                "➕ Create New Exam",
                "✏️ Edit Exam",
                "🗑️ Delete Exam",
                "🎓 As Student",
                "🚪 Logout"
            });
            if (choice =="__ESC__")
                continue;

            switch(choice)
            {
                case "👥 View All Users":
                    ViewAllUsers();
                    break;
                case "📝 View All Exams":
                    ViewAllExams();
                    break;
                case "📊 View All Results":
                    ViewAllResults();
                    break;
                case "📈 View Statistics":
                    ViewStatistics();
                    break;
                case "➕ Create New Exam":
                    router.Navigate("createexam");
                    return;
                case "✏️ Edit Exam":
                    router.Navigate("editexam");
                    return;
                case "🗑️ Delete Exam":
                    router.Navigate("deleteexam");
                    return;
                case "🎓 As Student":
                    router.Navigate("main");
                    return;
                case "🚪 Logout":
                    AppState.CurrentUser = null;
                    Print.SuccessMsg("✓ Logged out successfully!");
                    Thread.Sleep(1000);
                    router.Navigate("home");
                    return;
            }

            Console.Clear();
        }
    }

    private void ViewAllUsers()
    {
        Print.OutLine("👥 ALL USERS", ConsoleColor.Cyan);
        Console.WriteLine();

        var users =DataManager.UserDB.GetAll();

        if(users.Count==0)
        {
            Print.OutLine("No users registered.", ConsoleColor.Yellow);
        }
        else
        {
            Console.WriteLine($"Total Users: {users.Count}\n");
            foreach (var user in users)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"• {user.Username} ({user.Email})");
                Console.ResetColor();
                Console.WriteLine($"  Type: {(user.IsAdmin ? "Admin" : "Student")}");
                Console.WriteLine($"  Registered: {user.RegistrationDate:yyyy-MM-dd}");
                Console.WriteLine($"  Exams Taken: {user.ExamHistory.Count}");
                Console.WriteLine();
            }
        }
        Print.Pause();
    }

    private void ViewAllExams()
    {
        Print.OutLine("📝 ALL EXAMS", ConsoleColor.Cyan);
        Console.WriteLine();

        var exams =DataManager.ExamDB.GetAll();
        var subjects =DataManager.SubjectDB.GetAll();

        if(exams.Count==0)
        {
            Print.OutLine("No exams available.", ConsoleColor.Yellow);
        }
        else
        {
            Console.WriteLine($"Total Exams: {exams.Count}\n");
            foreach (var exam in exams)
            {
                var subject = subjects.FirstOrDefault(s => s.Id == exam.SubjectId);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"• {exam.Title}");
                Console.ResetColor();
                Console.WriteLine($"  Subject: {subject?.Name ?? "Unknown"}");
                Console.WriteLine($"  Type: {exam.Type}");
                Console.WriteLine($"  Questions: {exam.Questions.Count}");
                Console.WriteLine($"  Time Limit: {exam.TimeLimit} minutes");
                Console.WriteLine($"  Total Marks: {exam.GetTotalMarks()}");
                Console.WriteLine();
            }
        }

        Print.Pause();
    }

    private void ViewAllResults()
    {
        Print.OutLine("📊 ALL RESULTS", ConsoleColor.Cyan);
        Console.WriteLine();

        var results =DataManager.ResultDB.GetAll();
        var users =DataManager.UserDB.GetAll();
        var exams =DataManager.ExamDB.GetAll();
        var subjects =DataManager.SubjectDB.GetAll();

        if(results.Count== 0)
        {
            Print.OutLine("No results available.", ConsoleColor.Yellow);
        }
        else
        {
            Console.WriteLine($"Total Results: {results.Count}\n");
            foreach (var result in results.OrderByDescending(r => r.DateTaken).Take(20))
            {
                var user =users.FirstOrDefault(u => u.Id == result.UserId);
                var exam =exams.FirstOrDefault(e => e.Id == result.ExamId);
                var subject =subjects.FirstOrDefault(s => s.Id == exam?.SubjectId);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"• {user?.Username ?? "Unknown"} - {subject?.Name ?? "Unknown"}");
                Console.ResetColor();
                Console.WriteLine($"  Score: {result.Score}/{result.TotalMarks} ({result.Percentage:F2}%)");
                Console.WriteLine($"  Grade: {result.Grade} | Status: {(result.Passed ? "PASSED ✓" : "FAILED ✗")}");
                Console.WriteLine($"  Date: {result.DateTaken:yyyy-MM-dd HH:mm}");
                Console.WriteLine();
            }

            if(results.Count >20)
            {
                Console.WriteLine($"... and {results.Count - 20} more results");
            }
        }
        Print.Pause();
    }

    private void ViewStatistics()
    {
        Print.OutLine("📊 SYSTEM STATISTICS", ConsoleColor.Cyan);
        Console.WriteLine();

        var results =DataManager.ResultDB.GetAll();
        var users =DataManager.UserDB.GetAll();
        var exams =DataManager.ExamDB.GetAll();
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Total Users: {users.Count}");
        Console.WriteLine($"Total Exams: {exams.Count}");
        Console.WriteLine($"Total Results: {results.Count}");
        
        if(results.Count >0)
        {
            Console.WriteLine($"Average Score: {results.Average(r => r.Percentage):F2}%");
            Console.WriteLine($"Pass Rate: {(results.Count(r => r.Passed) * 100.0 / results.Count):F2}%");
        }
        Console.ResetColor();
        Console.WriteLine();
        Print.Pause();
    }
}