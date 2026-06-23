using System;
using System.Collections.Generic;

namespace CybersecurityChatbotPart2
{
    public static class ActivityLogger
    {
        private static readonly List<string> _log = new List<string>();
        private const int MaxLogEntries = 20;

        public static void Log(string action)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {action}";
            _log.Add(entry);
            if (_log.Count > MaxLogEntries)
                _log.RemoveAt(0);
        }

        public static List<string> GetLog(int count = 10)
        {
            int start = Math.Max(0, _log.Count - count);
            return _log.GetRange(start, _log.Count - start);
        }

        public static void Clear() => _log.Clear();
    }
}
