using MID_PROJ.Routes;
using MID_PROJ.Utils;
using Spectre.Console;

namespace MID_PROJ.Pages;

public class HomePage : Page
{
    public override void Display()
    {
        Console.Clear();
      Print.OutLine("╔═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗", ConsoleColor.DarkCyan);
      Print.OutLine("║                                                    📚  W E L C O M E   T O... 📚                                              ║", ConsoleColor.Cyan);
      Print.OutLine("║                                                                                                                               ║", ConsoleColor.Cyan);
      Print.OutLine("║                ███████╗ ██╗  ██╗  █████╗  ███╗   ███╗ ██╗ ███╗   ██╗  █████╗  ████████╗ ██╗ ██████╗  ███╗   ██╗               ║", ConsoleColor.Cyan);
      Print.OutLine("║                ██╔════╝ ╚██╗██╔╝ ██╔══██╗ ████╗ ████║ ██║ ████╗  ██║ ██╔══██╗ ╚══██╔══╝ ██║ ██╔══██╗ ████╗  ██║               ║", ConsoleColor.Blue);
      Print.OutLine("║                █████╗    ╚███╔╝  ███████║ ██╔████╔██║ ██║ ██╔██╗ ██║ ███████║    ██║    ██║ ██║  ██║ ██╔██╗ ██║               ║", ConsoleColor.Cyan);
      Print.OutLine("║                ██╔══╝    ██╔██╗  ██╔══██║ ██║╚██╔╝██║ ██║ ██║╚██╗██║ ██╔══██║    ██║    ██║ ██║  ██║ ██║╚██╗██║               ║", ConsoleColor.Blue);
      Print.OutLine("║                ███████╗ ██╔╝ ██╗ ██║  ██║ ██║ ╚═╝ ██║ ██║ ██║ ╚████║ ██║  ██║    ██║    ██║ ██████╔╝ ██║ ╚████║               ║", ConsoleColor.Cyan);
      Print.OutLine("║                ╚══════╝ ╚═╝  ╚═╝ ╚═╝  ╚═╝ ╚═╝     ╚═╝ ╚═╝ ╚═╝  ╚═══╝ ╚═╝  ╚═╝    ╚═╝    ╚═╝ ╚═════╝  ╚═╝  ╚═══╝               ║", ConsoleColor.Gray);
      Print.OutLine("║                                                                                                                               ║", ConsoleColor.Cyan);
      Print.OutLine("║                                  ███████╗ ██╗   ██╗ ███████╗ ████████╗ ███████╗ ███╗   ███╗                                   ║", ConsoleColor.DarkGray);
      Print.OutLine("║                                  ██╔════╝ ╚██╗ ██╔╝ ██╔════╝ ╚══██╔══╝ ██╔════╝ ████╗ ████║                                   ║", ConsoleColor.DarkGray);
      Print.OutLine("║                                  ███████╗  ╚████╔╝  ███████╗    ██║    █████╗   ██╔████╔██║                                   ║", ConsoleColor.DarkGray);
      Print.OutLine("║                                  ╚════██║   ╚██╔╝   ╚════██║    ██║    ██╔══╝   ██║╚██╔╝██║                                   ║", ConsoleColor.DarkGray);
      Print.OutLine("║                                  ███████║    ██║    ███████║    ██║    ███████╗ ██║ ╚═╝ ██║                                   ║", ConsoleColor.DarkGray);
      Print.OutLine("║                                  ╚══════╝    ╚═╝    ╚══════╝    ╚═╝    ╚══════╝ ╚═╝     ╚═╝                                   ║", ConsoleColor.DarkGray);
      Print.OutLine("║                                                                                                                               ║", ConsoleColor.Cyan);
      Print.OutLine("║                                         Examination System – Test Your Knowledge! 📝                                          ║", ConsoleColor.DarkCyan);
      Print.OutLine("║                                            ⏱️ . 📄 . Fair & Accurate . 📊 . ✔️                                                  ║", ConsoleColor.DarkCyan);
      Print.OutLine("╚═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝", ConsoleColor.DarkCyan);

Print.OutLine("", ConsoleColor.White);
Print.OutLine("✨EXAMINATION SYSTEM — Where Knowledge is Tested and Excellence is Achieved✨", ConsoleColor.DarkCyan);
Print.OutLine("", ConsoleColor.White);
Print.OutLine("➡️ Press any key to begin your examination journey...🧭", ConsoleColor.Gray);
Console.ReadKey();
        AnsiConsole.MarkupLine("\n[bold gold1]✨Your legend of learning starts here!✨[/]\n");
        AnsiConsole.MarkupLine("[dim gray]Examination System - Academic Excellence and Innovation...[/]");
    }
    public override void HandleInput(Router router)
    {
        while(true)
        {
            Console.WriteLine();
            var input = Print.AskChoice("[bold cyan]\nSelect an option:[/]", new List<string>
            {
                "📝 Sign up",
                "🔐 Log in",
                "ℹ️  About",
                "❌ Exit"
            }); 

            if (input == "__ESC__")
                continue;

            switch(input)
            {
                case "📝 Sign up":
                    router.Navigate("register");
                    return;
                case "🔐 Log in":
                    router.Navigate("login");
                    return;
                case "ℹ️  About":
                    router.Navigate("about");
                    return;
                case "❌ Exit":
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n╔═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
                    Console.WriteLine("║                                                                                                                               ║");
                    Console.WriteLine("║                           Thank you for using the Examination System! 🎓                                                      ║");
                    Console.WriteLine("║                                                                                                                               ║");
                    Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");
                    Console.ResetColor();
                    Thread.Sleep(1500);
                    Environment.Exit(0);
                    return;
            }
        }
    }
}