using System;
using System.Collections.Generic;

namespace CybersecurityChatbotPart2
{
    public class ChatBotEngine
    {
        private readonly Dictionary<string, string> _keywordResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "phishing", "[ALERT] Don't click suspicious links. Verify the sender." },
            { "passwords", "[TIP] Use long, unique passwords + a password manager." },
            { "malware", "[ALERT] Ensure your antivirus is updated and avoid untrusted downloads." },
            { "social engineering", "[INFO] Be cautious of people manipulating you into giving up confidential info." },
            { "privacy", "[TIP] Review app permissions regularly and limit sharing personal data." }
        };

        public string GetResponse(string rawInput)
        {
            string input = rawInput.Trim().ToLower();

            foreach (var key in _keywordResponses.Keys)
            {
                if (input.Contains(key))
                {
                    return _keywordResponses[key];
                }
            }

            return "I'm here to help with cybersecurity topics. Try asking about: phishing, passwords, malware, social engineering, or privacy.";
        }
    }
}
