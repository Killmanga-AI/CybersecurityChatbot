using System;
using System.Collections.Generic;

namespace CybersecurityChatbotPart2
{
    public class ChatBotEngine
    {
        private string? _lastTopic;
        private readonly Random _random = new Random();

        private readonly Dictionary<string, string> _keywordResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "phishing", "[ALERT] Don't click suspicious links. Verify the sender." },
            { "passwords", "[TIP] Use long, unique passwords + a password manager." },
            { "malware", "[ALERT] Ensure your antivirus is updated and avoid untrusted downloads." },
            { "social engineering", "[INFO] Be cautious of people manipulating you into giving up confidential info." },
            { "privacy", "[TIP] Review app permissions regularly and limit sharing personal data." }
        };

        private readonly List<string> _phishingTips = new List<string>
        {
            "Verify the sender's email address by hovering over the display name.",
            "Never click on urgent links demanding immediate attention.",
            "Look out for spelling or grammatical errors in official-looking emails."
        };

        private readonly List<string> _passwordTips = new List<string>
        {
            "Create phrases instead of single words, like 'CorrectHorseBatteryStaple'.",
            "Avoid reusing the same password across multiple platforms.",
            "Use a reputable password manager to generate and store complex passwords."
        };

        public string GetResponse(string rawInput)
        {
            string input = rawInput.Trim().ToLower();

            // Follow-up logic
            if (System.Text.RegularExpressions.Regex.IsMatch(input, @"tell me more|another tip|explain more|more info|elaborate|continue|what else", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                if (_lastTopic != null)
                {
                    if (_lastTopic == "phishing") return GetRandomTip("phishing");
                    if (_lastTopic == "passwords") return GetRandomTip("passwords");
                    if (_keywordResponses.ContainsKey(_lastTopic)) return _keywordResponses[_lastTopic];
                }
                return "What topic do you want more info about?";
            }

            foreach (var key in _keywordResponses.Keys)
            {
                if (input.Contains(key))
                {
                    _lastTopic = key;
                    if ((key == "phishing" || key == "passwords") && input.Contains("tip"))
                        return GetRandomTip(key);
                    return _keywordResponses[key];
                }
            }

            return "I'm here to help with cybersecurity topics. Try asking about: phishing, passwords, malware, social engineering, or privacy.";
        }

        private string GetRandomTip(string topic)
        {
            if (topic == "phishing") return "[TIP] " + _phishingTips[_random.Next(_phishingTips.Count)];
            if (topic == "passwords") return "[TIP] " + _passwordTips[_random.Next(_passwordTips.Count)];
            return "";
        }
    }
}
