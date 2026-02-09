using MID_PROJ.Routes;
using MID_PROJ.Services;
using MID_PROJ.Models;
using MID_PROJ.Utils;

namespace MID_PROJ.Pages;
public class ProfilePage : Page
{
    public override void Display()
    {
        var welcomeName = AppState.CurrentUser?.IsAdmin == true ? "ELOSTORA" : AppState.CurrentUser?.Username;
        if (!string.IsNullOrWhiteSpace(welcomeName))
        {
            Print.OutLine($"👋 Welcome {welcomeName}", ConsoleColor.Yellow);
        }
        Print.OutLine("👤 USER PROFILE", ConsoleColor.DarkCyan);
        Console.WriteLine(new string('═', 60));
    }
    public override void HandleInput(Router router)
    {
        if(AppState.CurrentUser==null)
        {
            router.Navigate("home");
            return;
        }

        Print.PrintFixedESCMessage();

        Console.WriteLine();
        var user =AppState.CurrentUser;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Username: {user.Username}");
        Console.WriteLine($"Email: {user.Email}");
        Console.WriteLine($"Account Type: {(user.IsAdmin ? "Admin" : "Student")}");
        Console.WriteLine($"Registration Date: {user.RegistrationDate:yyyy-MM-dd}");
        Console.WriteLine($"Exams Taken: {user.ExamHistory.Count}");
        Console.ResetColor();
        Console.WriteLine();
        Print.CancelableInput(out _, "Press Enter to return to the main menu (ESC to cancel): ");
        router.Navigate("main");
    }
}