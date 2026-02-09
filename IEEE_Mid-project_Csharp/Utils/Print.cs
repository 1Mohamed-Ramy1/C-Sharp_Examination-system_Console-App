namespace MID_PROJ.Utils;
using System;
using Spectre.Console;
public static class Print
{
    public static void Invalid() { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Invalid input. Press any key..."); Console.ResetColor(); Console.ReadKey(); }
    public static void ErrorMsg(string msg) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(msg + " Please try again."); Console.ResetColor(); Console.ReadKey(); }
    public static void SuccessMsg(string msg) { Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine(msg + ". Press any key..."); Console.ResetColor(); Console.ReadKey(); }
    public static void Pause() { Console.WriteLine("Press any key to continue..."); Console.ReadKey(); }
    public static void Pausecounter(int seconds = 3)
    {
        for (int i = seconds; i >= 1; i--)
        {
            Console.Write($"\rReturning in [{new string('■', seconds - i + 1)}{new string(' ', i - 1)}] {i}s ");
            Thread.Sleep(3000);
        }
        Console.Clear();
    }
    public static void Pausecounteradmin(int seconds = 3)
    {
        for (int i = seconds; i >= 1; i--)
        {
            Console.Write($"\rReturning in [{new string('■', seconds - i + 1)}{new string(' ', i - 1)}] {i}s ");
            Thread.Sleep(1000);
        }
        Console.Clear();
    }

    public static void OutLine(string message, ConsoleColor color = ConsoleColor.White)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = originalColor;
    }
    public static void Out(string message, ConsoleColor color = ConsoleColor.White)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(message);
        Console.ForegroundColor = originalColor;
    }


    public static bool AskYesNo(string message, bool defaultYes = true)
    {
        while (true)
        {
            Out(message + (defaultYes ? " [Y/n]" : " [y/N]"), ConsoleColor.Cyan);
            Console.Write("> ");
            var input = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrEmpty(input)) return defaultYes;
            if (input == "y" || input == "yes") return true;
            if (input == "n" || input == "no") return false;

            OutLine("Invalid input. Please enter Y or N.", ConsoleColor.Red);
        }
    }

    public static string Ask(string message, ConsoleColor color = ConsoleColor.Cyan)
    {
        Out(message, color);
        return Console.ReadLine()?.Trim() ?? "";
    }

    public static int AskNumber(string message, int? min = null, int? max = null)
    {
        while (true)
        {
            var rangeMsg = (min != null || max != null)
                ? $" ({min ?? int.MinValue} - {max ?? int.MaxValue})"
                : "";

            Out($"{message}{rangeMsg}", ConsoleColor.Cyan);
            var input = Console.ReadLine();

            if (int.TryParse(input, out int number))
            {
                if ((min == null || number >= min) && (max == null || number <= max))
                    return number;
            }

            OutLine("Invalid input. Please enter a valid number.", ConsoleColor.Red);
        }
    }
    public static string AskChoice(string prompt, List<string> options, bool allowBack = false)
    {
        var menuOptions = new List<string>(options);
        if (allowBack)
            menuOptions.Add("⬅ Back");
        try
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(prompt)
                    .PageSize(10)
                    .AddChoices(menuOptions)
            );

            return choice;
        }
        catch (OperationCanceledException)
        {
            return allowBack ? "⬅ Back" : "__ESC__";
        }
    }
    internal static string AskHidden(string prompt)
    {
        Console.Write(prompt);
        string input = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input = input[..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                input += key.KeyChar;
                Console.Write("*");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return input;
    }
    public static bool CancelableInput(out string result, string prompt, bool secret = false)
    {
        result = "";

        Console.Write(prompt);
        var input = "";
        ConsoleKeyInfo keyInfo;
        while (true)
        {
            keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Enter)
                break;

            if (keyInfo.Key == ConsoleKey.Escape)
                return true;

            if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input = input[..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                input += keyInfo.KeyChar;
                Console.Write(secret ? '*' : keyInfo.KeyChar);
            }
        }
        Console.WriteLine();
        result = input.Trim();
        return false;
    }

    public static bool CancelableNumber<T>(out T result, string prompt) where T : struct, IConvertible
    {
        result = default!;
        while (true)
        {
            if (CancelableInput(out string input, prompt)) return true;
            if (string.IsNullOrWhiteSpace(input)) continue;
            try
            {
                result = (T)Convert.ChangeType(input, typeof(T));
                return false;
            }
            catch
            {
                Print.OutLine("Invalid number, try again!", ConsoleColor.Red);
            }
        }
    }

    public static void PrintFixedESCMessage()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════╗");
        Console.WriteLine("║ 🔹 Press ESC anytime to cancel and return to menu");
        Console.WriteLine("╚════════════════════════════════════════════════════╝\n");
    }
}
