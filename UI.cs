using System;

public static class UI
{
    public static void DisplayHeader()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
         ____  ____  ____  ___  _____  ____
        / __ \(  _ \(  _ \(_ _)(__  __)(_  _)
       ( (_) ) )   / ) _ <  ) (   / /    )(  
        \____/(_)\_)(____/(____) (___)  (__) 
        ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("      >> Orbiting Your Digital Defense <<");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("=================================================");
        Console.ResetColor();
    }

    public static string GetUserName()
    {
        Console.Write("\n[?] Enter your name: ");
        string name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name)) name = "User";
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