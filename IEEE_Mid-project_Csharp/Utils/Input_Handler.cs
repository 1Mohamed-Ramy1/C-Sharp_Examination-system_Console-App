namespace MID_PROJ.Utils;

public class Input_Handler
{
    public static string Read_Non_Empty_String(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
                return input;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid input. It cannot be empty.");
            Console.ResetColor();
        }
    }

    public static double Read_Double(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (double.TryParse(input, out double value) && !string.IsNullOrWhiteSpace(input))
                return value;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid input. Please enter a valid number (decimal allowed).");
            Console.ResetColor();
        }
    }

    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value))
                return value;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid input. Please enter a valid integer.");
            Console.ResetColor();
        }
    }

    public static int Read_Int_In_Range(string prompt, int min, int max)
    {
        while (true)
        {
            int value = ReadInt(prompt);

            if (value >= min && value <= max)
                return value;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Invalid input. Enter a number between {min} and {max}.");
            Console.ResetColor();
        }
    }
}
