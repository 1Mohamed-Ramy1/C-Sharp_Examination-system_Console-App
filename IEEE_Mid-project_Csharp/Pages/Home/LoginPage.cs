using MID_PROJ.Models;
using MID_PROJ.Routes;
using MID_PROJ.Services;
using MID_PROJ.Utils;
using Spectre.Console;

namespace MID_PROJ.Pages;
public class LoginPage : Page
{
    private static string? LastUsedEmail = null;
    public override void Display()
    {
        AnsiConsole.Clear();
        Print.OutLine("🔐 LOGIN TO YOUR ACCOUNT", ConsoleColor.DarkCyan);
        Console.WriteLine(new string('═', 40));
    }
    public override void HandleInput(Router router)
    {
        while(true)
        {
            AnsiConsole.Clear();
            Display();
            Print.PrintFixedESCMessage();
            Print.OutLine("⚠️  Note: When you register the account, do not forget at the end (@edu.com)", ConsoleColor.Yellow);
            Console.WriteLine();
            Console.WriteLine(new string('═', 40));
            Console.WriteLine();

            Console.WriteLine();
            var step = 0;
            string email = "";
            string password = "";

            while (true)
            {
                if (step == 0)
                {
                    if (LastUsedEmail != null)
                    {
                        var prompt = $"📨 Email (Press Enter to use last: {LastUsedEmail}) => ";
                        if (Print.CancelableInput(out string tempInput, prompt))
                        {
                            router.Navigate("home");
                            return;
                        }
                        email = string.IsNullOrWhiteSpace(tempInput) ? LastUsedEmail : tempInput!;
                    }
                    else
                    {
                        if (Print.CancelableInput(out string tempInput, "📨 Email => "))
                        {
                            router.Navigate("home");
                            return;
                        }
                        email = tempInput.Trim();
                    }

                    if (string.IsNullOrWhiteSpace(email))
                    {
                        Print.OutLine("❌ Email cannot be empty. Please try again.", ConsoleColor.Red);
                        continue;
                    }

                    step = 1;
                    Console.WriteLine();
                }
                else
                {
                    if (Print.CancelableInput(out string tempPassword, "🔑 Password: ", secret: true))
                    {
                        step = 0;
                        Console.WriteLine();
                        continue;
                    }
                    password = tempPassword;
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        Print.OutLine("❌ Password cannot be empty. Please try again.", ConsoleColor.Red);
                        continue;
                    }
                    break;
                }
            }

            Console.WriteLine();
            var usersList =DataManager.UserDB.GetAll();
            var user =usersList.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                u.Password ==password);

            if (AdminManager.IsAdmin(email, password))
            {
                AppState.CurrentUser = AdminManager.GetAdmin();
                Print.SuccessMsg("🎉 Welcome Boss..👑");
                Console.WriteLine();
                router.Navigate("admin");
                return;
            }

            if(user ==null)
            {
                Print.OutLine("❌ Email or Password is incorrect. Please try again.",ConsoleColor.Red);
                ContinueOrBack(router);
                continue;
            }
            AppState.CurrentUser = user;
            Print.SuccessMsg($"👋 Welcome : {user.Username}!");
            Console.WriteLine();

            if(!user.IsAdmin)
                LastUsedEmail = user.Email;

            router.Navigate(user.IsAdmin ? "admin" : "main");
            return;
        }
    }
    private void ContinueOrBack(Router router)
    {
        var choice = Print.AskChoice("What do you want to do next ❓", new List<string>
        {
            "🔁 Try Again",
            "🏠 Back to Home"
        });

        if (choice =="🏠 Back to Home")
            router.Navigate("home");
    }
}