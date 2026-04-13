using System;
using System.IO;
using System.Media;

public static class AudioPlayer
{
    public static void PlayGreeting()
    {
        string exeDir = AppContext.BaseDirectory;
        string cwd = Directory.GetCurrentDirectory();
        string[] candidates =
        {
            Path.Combine(exeDir, "greeting.wav"),
            Path.Combine(cwd, "greeting.wav")
        };

        string? found = null;
        foreach (string c in candidates)
        {
            if (File.Exists(c))
            {
                found = c;
                break;
            }
        }

        if (found is null)
        {
            Console.WriteLine("[!] greeting.wav not found. Checked:");
            Console.WriteLine($"    {candidates[0]}");
            Console.WriteLine($"    {candidates[1]}");
            Console.WriteLine("    Ensure the file is included in the project and set to 'Copy to Output Directory'.");
            return;
        }

        try
        {
            using SoundPlayer player = new SoundPlayer(found);
            player.Load();
            player.PlaySync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[!] Audio playback failed: {ex.GetType().Name}: {ex.Message}");
        }
    }
}
