using System;

public class Chatbot
{
    private string userName;

    public Chatbot(string name)
    {
        userName = name;
    }

    public void Start()
    {
        Console.WriteLine("Type 'help' for topics or 'exit' to quit.");
        while (true)
        {
            UI.PrintDivider();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{userName} > ");
            string input = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Empty input. Please type something.");
                continue;
            }

            if (input == "exit")
            {
                Console.WriteLine("\nStay safe online. Goodbye!");
                break;
            }

            Respond(input);
        }
    }

    private void Respond(string input)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;

        if (input.Contains("how are you"))
            Console.WriteLine("I'm fully operational and ready to protect you.");
        else if (input.Contains("purpose") || input.Contains("what can i ask"))
            Console.WriteLine("Ask me about: passwords, phishing, or safe browsing.");
        else if (input.Contains("password"))
            Console.WriteLine("[TIP] Use long, unique passwords + a password manager.");
        else if (input.Contains("phishing"))
            Console.WriteLine("[ALERT] Don't click suspicious links. Verify the sender.");
        else if (input.Contains("safe browsing") || input.Contains("browsing"))
            Console.WriteLine("[INFO] Look for HTTPS and avoid unknown downloads.");
        else if (input.Contains("help"))
            Console.WriteLine("Topics: password, phishing, safe browsing, how are you, purpose");
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("I didn't understand. Try 'help' for examples.");
        }
        Console.ResetColor();
    }
}