using MID_PROJ.Routes;
using MID_PROJ.Services;
using MID_PROJ.Models;
using MID_PROJ.Utils;

namespace MID_PROJ.Pages;
public class StatisticsPage : Page
{
    public override void Display()
    {
        Print.OutLine("📊 STATISTICS", ConsoleColor.DarkCyan);
        Console.WriteLine(new string('═', 60));
    }

    public override void HandleInput(Router router)
    {
        if(AppState.CurrentUser ==null)
        {
            router.Navigate("home");
            return;
        }
        Print.PrintFixedESCMessage();
        Console.WriteLine();
        var results =DataManager.ResultDB.GetAll();
        var users =DataManager.UserDB.GetAll();
        var exams =DataManager.ExamDB.GetAll();
        Console.ForegroundColor =ConsoleColor.Cyan;
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
        router.Navigate("main");
    }
}