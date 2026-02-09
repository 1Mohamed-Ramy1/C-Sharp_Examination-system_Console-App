using MID_PROJ.Routes;
using MID_PROJ.Services;
using MID_PROJ.Models;
using MID_PROJ.Utils;
using Spectre.Console;

namespace MID_PROJ.Pages;
public class RegisterPage : Page
{
    public override void Display()
    {
        Print.OutLine("📝 CREATE YOUR ACCOUNT", ConsoleColor.DarkCyan);
        Console.WriteLine(new string('═', 40));
    }

    public override void HandleInput(Router router)
    {
        while(true)
        {
            Console.Clear();
            Display();
            Print.PrintFixedESCMessage();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("⚠️  NOTE: When you register the account, don't forget to include '@edu.com' at the end of your email.⚠️");
            Console.WriteLine();
            Console.WriteLine("⚠️  Password must be at least 4 characters and include: uppercase, lowercase, and number.⚠️");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("\n════════════════════════════════════════");
            Console.WriteLine();

            int step = 0;
            string username = "";
            string email = "";
            string password = "";

            while (true)
            {
                if (step == 0)
                {
                    if (Print.CancelableInput(out string tempUsername, "Username => (Your Name) : "))
                    {
                        router.Navigate("home");
                        return;
                    }
                    username = tempUsername.Trim();
                    if (string.IsNullOrWhiteSpace(username))
                    {
                        Print.ErrorMsg("Username cannot be empty ❌");
                        continue;
                    }
                    step = 1;
                    Console.WriteLine();
                }
                else if (step == 1)
                {
                    if (Print.CancelableInput(out string tempEmail, "Email: "))
                    {
                        step = 0;
                        Console.WriteLine();
                        continue;
                    }
                    email = tempEmail.Trim();
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        Print.ErrorMsg("Email cannot be empty ❌");
                        continue;
                    }
                    if (!email.EndsWith("@edu.com"))
                    {
                        Print.ErrorMsg("Email must end with @edu.com ❌");
                        continue;
                    }
                    step = 2;
                    Console.WriteLine();
                }
                else
                {
                    if (Print.CancelableInput(out string tempPassword, "Password: ", secret: true))
                    {
                        step = 1;
                        Console.WriteLine();
                        continue;
                    }
                    password = tempPassword;
                    if (password.Length < 4 ||
                        !password.Any(char.IsUpper) ||
                        !password.Any(char.IsLower) ||
                        !password.Any(char.IsDigit))
                    {
                        Print.ErrorMsg("❌ Password must be at least 4 characters and include: uppercase, lowercase, and number.");
                        continue;
                    }

                    if (Print.CancelableInput(out string confirm, "Confirm Password: ", secret: true))
                    {
                        step = 1;
                        Console.WriteLine();
                        continue;
                    }
                    if (confirm != password)
                    {
                        Print.ErrorMsg("❌ Passwords do not match.");
                        continue;
                    }
                    break;
                }
            }
            
            var users =DataManager.UserDB.GetAll();
            var existingUser =users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if(existingUser !=null)
            {
                Print.ErrorMsg("⚠️ Account already exists");
                var reSignup = Print.AskYesNo("Do you want to try signup again?");
                if (reSignup) continue;
                router.Navigate("home");
                return;
            }
            if(Print.AskYesNo("👁️ Do you want to view your password before saving?"))
            {
                Console.WriteLine();
                Console.WriteLine($"🔐 Your password: {password}");
                Console.WriteLine();
                if (!Print.AskYesNo("Is this correct?"))
                {
                    Print.OutLine("🔁 Let's try again...", ConsoleColor.Yellow);
                    Console.WriteLine();
                    continue;
                }
            }
            
            DataManager.UserDB.Add(new User(username, email, password));
            Console.WriteLine();
            Print.SuccessMsg("✅ Account created successfully 🎉");
            Console.WriteLine();
            var goToLogin = Print.AskYesNo("Do you want to login now?");
            Console.WriteLine();
            if(goToLogin)
            {
                router.Navigate("login");
                return;
            }
            router.Navigate("home");
            return;
        }
    }
}