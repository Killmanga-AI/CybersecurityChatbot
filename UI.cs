using System;

public static class UI
{
    public static void DisplayHeader()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
 ██████╗ ██████╗ ██████╗ ██╗████████╗
██╔═══██╗██╔══██╗██╔══██╗██║╚══██╔══╝
██║   ██║██████╔╝██████╔╝██║   ██║   
██║   ██║██╔══██╗██╔══██╗██║   ██║   
╚██████╔╝██║  ██║██████╔╝██║   ██║   
 ╚═════╝ ╚═╝  ╚═╝╚═════╝ ╚═╝   ╚═╝   
    ");

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("        ──[ ORBIT SECURITY SYSTEM ]──");

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("         >> Orbiting Your Digital Defense <<");

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("═════════════════════════════════════════════════");
        Console.ResetColor();
    }

    public static string GetUserName()
    {
        Console.Write("\n[?] Enter your name: ");
        string? raw = Console.ReadLine();
        string name = string.IsNullOrWhiteSpace(raw) ? "User" : raw.Trim();
        Console.Clear();
        DisplayHeader();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n[!] Welcome, {name}!\n");
        Console.ResetColor();
        return name;
    }

    public static void PrintDivider()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("-------------------------------------------------");
        Console.ResetColor();
    }
}
