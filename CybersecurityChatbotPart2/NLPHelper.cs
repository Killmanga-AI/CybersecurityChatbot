using System;
using System.Text.RegularExpressions;

namespace CybersecurityChatbotPart2
{
    public static class NLPHelper
    {
        public enum Intent
        {
            None,
            AddTask,
            SetReminder,
            ShowTasks,
            CompleteTask,
            DeleteTask,
            StartQuiz,
            ShowLog,
            Help
        }

        public static Intent DetectIntent(string userInput)
        {
            string input = userInput.ToLower();

            // Add task / reminder patterns
            if (Regex.IsMatch(input, @"\b(add|create|new)\s+(task|to do|item)\b") ||
                Regex.IsMatch(input, @"\b(remind me to|set reminder for|remind me about)\b") ||
                Regex.IsMatch(input, @"\b(add\s+reminder)\b") ||
                Regex.IsMatch(input, @"\b(set\s+up\s+2fa|enable\s+2fa)\b") ||
                Regex.IsMatch(input, @"\b(review\s+privacy)\b"))
            {
                return Intent.AddTask;
            }

            // Show tasks
            if (Regex.IsMatch(input, @"\b(show|list|view)\s+(my\s+)?tasks\b") ||
                Regex.IsMatch(input, @"\bwhat tasks do i have\b"))
                return Intent.ShowTasks;

            // Complete task
            if (Regex.IsMatch(input, @"\b(complete|done|finish)\s+task\b") ||
                Regex.IsMatch(input, @"\bmark\s+as\s+completed\b"))
                return Intent.CompleteTask;

            // Delete task
            if (Regex.IsMatch(input, @"\b(delete|remove)\s+task\b") ||
                Regex.IsMatch(input, @"\b(erase|clear)\s+task\b"))
                return Intent.DeleteTask;

            // Start quiz
            if (Regex.IsMatch(input, @"\b(start|begin|take)\s+(quiz|test|game)\b") ||
                Regex.IsMatch(input, @"\bquiz me\b"))
                return Intent.StartQuiz;

            // Show log
            if (Regex.IsMatch(input, @"\b(show|view)\s+(activity\s+)?log\b") ||
                Regex.IsMatch(input, @"\bwhat have you done for me\b") ||
                Regex.IsMatch(input, @"\brecent actions\b"))
                return Intent.ShowLog;

            // Help
            if (Regex.IsMatch(input, @"\bhelp\b") ||
                Regex.IsMatch(input, @"\bwhat can you do\b"))
                return Intent.Help;

            return Intent.None;
        }

        public static string ExtractTaskDetails(string input)
        {
            // Remove common prefixes
            string cleaned = Regex.Replace(input, @"(add|create|new)\s+(task|to do|item)\s+", "", RegexOptions.IgnoreCase);
            cleaned = Regex.Replace(cleaned, @"(remind me to|set reminder for|remind me about)\s+", "", RegexOptions.IgnoreCase);
            cleaned = Regex.Replace(cleaned, @"\b(set up|enable)\s+", "", RegexOptions.IgnoreCase);
            cleaned = Regex.Replace(cleaned, @"\b(review)\s+", "", RegexOptions.IgnoreCase);
            return cleaned.Trim();
        }

        public static DateTime? ExtractReminderDate(string input)
        {
            // "in 3 days", "tomorrow", "5 days from now"
            Match match = Regex.Match(input, @"in\s+(\d+)\s+(day|days|week|weeks)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                int num = int.Parse(match.Groups[1].Value);
                string unit = match.Groups[2].Value.ToLower();
                if (unit.StartsWith("day")) return DateTime.Now.AddDays(num);
                if (unit.StartsWith("week")) return DateTime.Now.AddDays(num * 7);
            }
            if (Regex.IsMatch(input, @"\btomorrow\b", RegexOptions.IgnoreCase))
                return DateTime.Now.AddDays(1);
            if (Regex.IsMatch(input, @"\bnext week\b", RegexOptions.IgnoreCase))
                return DateTime.Now.AddDays(7);
            return null;
        }
    }
}
