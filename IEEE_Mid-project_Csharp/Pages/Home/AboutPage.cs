using MID_PROJ.Routes;
using MID_PROJ.Utils;
using MID_PROJ.Services;

namespace MID_PROJ.Pages;
public class AboutPage : Page
{
    public override void Display()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("ADVANCED EXAMINATION MANAGEMENT SYSTEM");
        Console.WriteLine("======================================");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("This comprehensive examination system provides:");
        Console.WriteLine();
        Console.WriteLine("📝 Exam Management:");
        Console.WriteLine("   • Create Final and Practical exams");
        Console.WriteLine("   • Support for True/False and MCQ questions");
        Console.WriteLine("   • Flexible question creation and editing");
        Console.WriteLine();
        Console.WriteLine("👨‍🎓 Student Features:");
        Console.WriteLine("   • User registration and authentication");
        Console.WriteLine("   • Take exams with real-time timer");
        Console.WriteLine("   • View detailed results and feedback");
        Console.WriteLine("   • Track exam history and progress");
        Console.WriteLine();
        Console.WriteLine("📊 Analytics & Reporting:");
        Console.WriteLine("   • Comprehensive statistics dashboard");
        Console.WriteLine("   • Grade distribution analysis");
        Console.WriteLine("   • Top performers leaderboard");
        Console.WriteLine("   • Export functionality for reports");
        Console.WriteLine();
        Console.WriteLine("Developed as part of IEEE Backend Project");
        Console.WriteLine("Demonstrating OOP principles and best practices");
        Console.WriteLine();
    }

    public override void HandleInput(Router router)
    {
        Print.Pause();
        
        // Navigate back based on whether user is logged in
        if (AppState.CurrentUser != null)
        {
            router.Navigate("main");
        }
        else
        {
            router.Navigate("home");
        }
    }
}