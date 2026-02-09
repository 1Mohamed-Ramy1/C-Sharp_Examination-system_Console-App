using MID_PROJ.Routes;
using MID_PROJ.Services;
using MID_PROJ.Models;
using MID_PROJ.Utils;

namespace MID_PROJ.Pages;
public class SubjectsPage : Page
{
    public override void Display()
    {
        var welcomeName = AppState.CurrentUser?.IsAdmin == true ? "ELOSTORA" : AppState.CurrentUser?.Username;
        if (!string.IsNullOrWhiteSpace(welcomeName))
        {
            Print.OutLine($"👋 Welcome {welcomeName}", ConsoleColor.Yellow);
        }
        Print.OutLine("📚 AVAILABLE SUBJECTS", ConsoleColor.DarkCyan);
        Console.WriteLine(new string('═',60));
    }

    public override void HandleInput(Router router)
    {
        Print.PrintFixedESCMessage();
        Console.WriteLine();
        var subjects =DataManager.SubjectDB.GetAll();
        if(subjects.Count ==0)
        {
            Print.OutLine("No subjects available yet.", ConsoleColor.Yellow);
            Print.CancelableInput(out _, "Press Enter to return to the main menu (ESC to cancel): ");
            router.Navigate("main");
            return;
        }

        for(int i =0; i< subjects.Count;i++ )
        {
            var subject =subjects[i];
            Console.ForegroundColor =ConsoleColor.Cyan;
            Console.WriteLine($"\n{i + 1}. {subject.Name}");
            Console.ResetColor();

            if(subject.Topics.Count >0)
            {
                Console.WriteLine($"   Topics: {string.Join(", ", subject.Topics.Take(3))}...");
            }
        }

        Console.WriteLine();
        var choice = Print.AskChoice("Select Action", new List<string>
        {
            "Take Exam",
            "Back to Main Menu"
        });

        if(choice=="Back to Main Menu" || choice=="__ESC__")
        {
            router.Navigate("main");
            return;
        }

        router.Navigate("takeexam");
    }
}