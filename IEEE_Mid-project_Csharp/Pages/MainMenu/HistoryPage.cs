using MID_PROJ.Routes;
using MID_PROJ.Services;
using MID_PROJ.Models;
using MID_PROJ.Utils;

namespace MID_PROJ.Pages;
public class HistoryPage : Page
{
    public override void Display()
    {
        var welcomeName = AppState.CurrentUser?.IsAdmin == true ? "ELOSTORA" : AppState.CurrentUser?.Username;
        if (!string.IsNullOrWhiteSpace(welcomeName))
        {
            Print.OutLine($"👋 Welcome {welcomeName}", ConsoleColor.Yellow);
        }
        Print.OutLine("📜 EXAM HISTORY",ConsoleColor.DarkCyan);
        Console.WriteLine(new string('═',60));
    }

    public override void HandleInput(Router router)
    {
        if(AppState.CurrentUser == null)
        {
            router.Navigate("home");
            return;
        }
        Print.PrintFixedESCMessage();
        Console.WriteLine();
        var user =AppState.CurrentUser;
        var results =DataManager.ResultDB.GetAll()
            .Where(r =>r.UserId ==user.Id)
            .OrderByDescending(r =>r.DateTaken)
            .ToList();

        if(results.Count==0)
        {
            Print.OutLine("No exam history available.",ConsoleColor.Yellow);
        }
        else
        {
            var exams =DataManager.ExamDB.GetAll();
            var subjects =DataManager.SubjectDB.GetAll();

            Console.ForegroundColor =ConsoleColor.Cyan;
            for(int i =0; i<results.Count;i++ )
            {
                var result =results[i];
                var exam =exams.FirstOrDefault(e => e.Id==result.ExamId);
                var subject =subjects.FirstOrDefault(s => s.Id== exam?.SubjectId);

                Console.WriteLine($"{i+1}. {subject?.Name ?? "Unknown Subject"}");
                Console.WriteLine($"   Date: {result.DateTaken:yyyy-MM-dd HH:mm}");
                Console.WriteLine($"   Score: {result.Score}/{result.TotalMarks} ({result.Percentage:F2}%)");
                Console.WriteLine($"   Grade: {result.Grade}");
                Console.WriteLine($"   Status: {(result.Passed ? "PASSED ✓" : "FAILED ✗")}");
                Console.WriteLine();
            }
            Console.ResetColor();
        }
        Print.CancelableInput(out _, "Press Enter to return to the main menu (ESC to cancel): ");
        router.Navigate("main");
    }
}