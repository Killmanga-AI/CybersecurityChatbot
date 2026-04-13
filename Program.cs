using System;

class Program
{
    static void Main(string[] args)
    {
        UI.DisplayHeader();
        AudioPlayer.PlayGreeting();
        string userName = UI.GetUserName();
        Chatbot bot = new Chatbot(userName);
        bot.Start();
    }
}