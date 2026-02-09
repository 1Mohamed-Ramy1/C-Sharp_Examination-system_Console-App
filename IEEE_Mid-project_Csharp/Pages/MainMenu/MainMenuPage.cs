using MID_PROJ.Routes;
using MID_PROJ.Services;
using MID_PROJ.Utils;

namespace MID_PROJ.Pages;
public class MainMenuPage : Page
{
    public override void Display()
    {
        if(AppState.CurrentUser==null)
            return;

        var welcomeName = AppState.CurrentUser.IsAdmin ? "ELOSTORA" : AppState.CurrentUser.Username;
        Print.OutLine($"👋 Welcome {welcomeName}", ConsoleColor.Yellow);
        Print.OutLine("📋 MAIN MENU", ConsoleColor.DarkCyan);
        Console.WriteLine(new string('═', 60));
    }

    public override void HandleInput(Router router)
    {
        if(AppState.CurrentUser==null)
        {
            router.Navigate("home");
            return;
        }

        var options = new List<string>
        {
            "📝 Take an Exam",
            "📚 View All Subjects",
            "👤 My Profile",
            "📖 My Exam History",
            "ℹ️ About"
        };

        if(AppState.CurrentUser.IsAdmin)
        {
            options.Insert(options.Count-1,"👨‍🏫 Teacher Panel");
        }

        options.Add("🚪 Logout");
        var choice =Print.AskChoice("Select an option:", options);
        if (choice == "__ESC__")
            return;
        switch(choice)
        {
            case "📝 Take an Exam":
                router.Navigate("takeexam");
                return;
            case "📚 View All Subjects":
                router.Navigate("subjects");
                return;
            case "👤 My Profile":
                router.Navigate("profile");
                return;
            case "📖 My Exam History":
                router.Navigate("history");
                return;
            case "ℹ️ About":
                router.Navigate("about");
                return;
            case "👨‍🏫 Teacher Panel":
                router.Navigate("admin");
                return;
            case "🚪 Logout":
                AppState.CurrentUser = null;
                Print.SuccessMsg("✓ Logged out successfully!");
                Thread.Sleep(1000);
                router.Navigate("home");
                return;
        }
    }
}