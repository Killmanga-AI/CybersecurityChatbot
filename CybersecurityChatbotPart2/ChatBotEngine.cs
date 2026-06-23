using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CybersecurityChatbotPart2
{
    public class ChatBotEngine
    {
        public delegate string SentimentAdjuster(string baseMessage, string sentiment);
        public SentimentAdjuster? AdjustMessageForSentiment { get; set; }

        private string? _lastTopic;
        private string? _userName;
        private string? _favoriteTopic;
        private readonly Random _random = new Random();
        private readonly TaskManager _taskManager;

        public ChatBotEngine()
        {
            AdjustMessageForSentiment = (baseMessage, sentiment) =>
            {
                switch (sentiment.ToLower())
                {
                    case "worried": return $"It's completely normal to feel concerned about this. {baseMessage}";
                    case "curious": return $"I'm glad you're interested in learning more! {baseMessage}";
                    case "frustrated": return $"I understand this can be frustrating. Let me help: {baseMessage}";
                    default: return baseMessage;
                }
            };
            string connStr = "server=localhost;user=root;password=yourpassword;database=cybersecurity_chatbot";
            _taskManager = new TaskManager(connStr);
        }

        // Dictionary
        private readonly Dictionary<string, string> _keywordResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "phishing", "[ALERT] Don't click suspicious links. Verify the sender." },
            { "passwords", "[TIP] Use long, unique passwords + a password manager." },
            { "malware", "[ALERT] Ensure your antivirus is updated and avoid untrusted downloads." },
            { "social engineering", "[INFO] Be cautious of people manipulating you." },
            { "privacy", "[TIP] Review app permissions regularly and limit sharing personal data." }
        };

        // Tip Lists
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
            if (TryExtractName(input, out string? name) && !string.IsNullOrEmpty(name))
            {
                _userName = char.ToUpper(name[0]) + name.Substring(1);
                return $"Nice to meet you, {_userName}! I'll remember your name.";
            }
            if (TryExtractFavoriteTopic(input, out string? topic))
            {
                _favoriteTopic = topic;
                return $"Great! I'll remember you're interested in {_favoriteTopic}.";
            }
            // Detect sentiment and apply adjustment
            string sentiment = DetectSentiment(input);
            string baseResponse = GenerateBaseResponse(input);
            string adjusted = AdjustMessageForSentiment?.Invoke(baseResponse, sentiment) ?? baseResponse;
            return PersonalizeResponse(adjusted);
        }

        private string GenerateBaseResponse(string input)
        {
            // Follow-up checking
            if (Regex.IsMatch(input, @"tell me more|another tip|explain more|more info|elaborate|continue|what else", RegexOptions.IgnoreCase))
            {
                if (_lastTopic != null)
                {
                    if (_lastTopic == "phishing") return GetRandomTip("phishing");
                    if (_lastTopic == "passwords") return GetRandomTip("passwords");
                    if (_keywordResponses.ContainsKey(_lastTopic)) return _keywordResponses[_lastTopic];
                }
                return "What topic do you want more info about?";
            }

            // Keyword checking
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

        private bool TryExtractName(string input, out string? name)
        {
            name = null;
            var match = Regex.Match(input, @"(?:my name is|call me|i am|i'm)\s+([a-z]+)", RegexOptions.IgnoreCase);
            if (match.Success) name = match.Groups[1].Value.ToLower();
            return match.Success;
        }

        private bool TryExtractFavoriteTopic(string input, out string? topic)
        {
            topic = null;
            var match = Regex.Match(input, @"(?:interested in|i like|i want to learn about)\s+(phishing|passwords|malware|social engineering|privacy)", RegexOptions.IgnoreCase);
            if (match.Success) topic = match.Groups[1].Value.ToLower();
            return match.Success;
        }

        private string PersonalizeResponse(string response)
        {
            string final = response;
            if (!string.IsNullOrEmpty(_userName) && !final.StartsWith(_userName))
                final = $"{_userName}, {final}";
            if (!string.IsNullOrEmpty(_favoriteTopic) && _lastTopic != _favoriteTopic)
                final += $" As someone interested in {_favoriteTopic}, you might find this especially useful.";
            return final;
        }

        public void ClearMemory()
        {
            _userName = null;
            _favoriteTopic = null;
            _lastTopic = null;
        }

        private string DetectSentiment(string input)
        {
            if (Regex.IsMatch(input, @"worried|concerned|scared|anxious|nervous|fear|afraid", RegexOptions.IgnoreCase)) return "worried";
            if (Regex.IsMatch(input, @"curious|wonder|learn|understand|interested|tell me about", RegexOptions.IgnoreCase)) return "curious";
            if (Regex.IsMatch(input, @"frustrated|annoyed|confused|don't understand|what does that mean", RegexOptions.IgnoreCase)) return "frustrated";
            return "neutral";
        }

        private string GetRandomTip(string topic)
        {
            if (topic == "phishing") return "[TIP] " + _phishingTips[_random.Next(_phishingTips.Count)];
            if (topic == "passwords") return "[TIP] " + _passwordTips[_random.Next(_passwordTips.Count)];
            return "";
        }
    }
}
