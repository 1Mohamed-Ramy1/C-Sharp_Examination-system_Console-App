namespace MID_PROJ.Utils;

public static class Arrow_Menu
{
    public static int arrow_meth(string title, string[] options, int width)
    {
        int menu_index = 0;
        ConsoleKey keyPressed;

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Title_Me(title, width);
            Console.ResetColor();

            for (int i = 0; i < options.Length; i++)
            {
                if (i == menu_index)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {options[i]}");
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;

            if (keyPressed == ConsoleKey.UpArrow && menu_index > 0)
            {
                menu_index--;
            }
            else if (keyPressed == ConsoleKey.DownArrow && menu_index < options.Length - 1)
            {
                menu_index++;
            }
            else if (keyPressed == ConsoleKey.Enter)
            {
                return menu_index;
            }
        }
    }

    public static void Title_Me(string title, int width)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(new string('─', width));
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(title);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(new string('─', width));
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;
    }
}
