using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CybersecurityChatbotPart2
{
    public class ChatBotEngine
    {
        public delegate string SentimentAdjuster(string baseMessage, string sentiment);
        public SentimentAdjuster? AdjustMessageForSentiment { get; set; }

        private bool _awaitingTaskReminder = false;
        private string _pendingTaskTitle = "";
        private bool _quizWaitingForAnswer = false;

        private string? _lastTopic;
        private string? _userName;
        private string? _favoriteTopic;
        private readonly Random _random = new Random();
        private readonly TaskManager _taskManager;
        private readonly QuizManager _quizManager;

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
            _quizManager = new QuizManager();
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
            // NLP intent detection (high priority)
            NLPHelper.Intent intent = NLPHelper.DetectIntent(rawInput);
            switch (intent)
            {
                case NLPHelper.Intent.AddTask:
                    string title = NLPHelper.ExtractTaskDetails(rawInput);
                    if (string.IsNullOrEmpty(title))
                        return "What task would you like to add? Please describe it.";

                    DateTime? reminder = NLPHelper.ExtractReminderDate(rawInput);
                    _taskManager.AddTask(title, reminder: reminder);
                    string response = $"Task added: '{title}'.";
                    if (reminder.HasValue)
                        response += $" Reminder set for {reminder.Value.ToShortDateString()}.";
                    else
                        response += " You can set a reminder later from the Task Manager.";
                    ActivityLogger.Log($"NLP: Task added via command.");
                    return response;

                case NLPHelper.Intent.ShowTasks:
                    var tasks = _taskManager.GetTasks();
                    if (tasks.Count == 0)
                        return "You have no pending tasks. Great job!";
                    string taskList = "Your pending tasks:\n";
                    foreach (var t in tasks)
                        taskList += $"- {t.Title}" + (t.ReminderDate.HasValue ? $" (Reminder: {t.ReminderDate.Value.ToShortDateString()})" : "") + "\n";
                    return taskList;

                case NLPHelper.Intent.StartQuiz:
                    _quizManager.StartQuiz();
                    _quizWaitingForAnswer = true;
                    var q = _quizManager.GetCurrentQuestion();
                    if (q != null)
                    {
                        string options = "";
                        for (int i = 0; i < q.Options.Count; i++)
                            options += $"{i + 1}. {q.Options[i]}\n";
                        return $"Quiz started!\n\n{q.Text}\n\n{options}\nType the number of your answer (1-{q.Options.Count}).";
                    }
                    return "Quiz started! Answer the questions.";

                case NLPHelper.Intent.ShowLog:
                    var log = ActivityLogger.GetLog(10);
                    if (log.Count == 0)
                        return "No recent activities recorded.";
                    string logText = "Recent activity log:\n";
                    foreach (var entry in log)
                        logText += $"- {entry}\n";
                    return logText;

                case NLPHelper.Intent.Help:
                    return "I can help you with:\n- Cybersecurity topics (phishing, passwords, malware, social engineering, privacy)\n- Task management (add task, show tasks)\n- Quiz (start quiz)\n- Activity log (show activity log)\n\nJust type your request naturally!";

                default:
                    // Check if quiz is active and user is answering
                    if (_quizWaitingForAnswer && _quizManager.IsActive)
                    {
                        if (int.TryParse(rawInput, out int choice) && choice >= 1 && choice <= 4)
                        {
                            bool correct = _quizManager.AnswerCurrent(choice - 1);
                            var currentQ = _quizManager.GetCurrentQuestion();
                            string feedback = correct ? "Correct! " : "Incorrect. ";
                            feedback += currentQ?.Explanation ?? "";

                            if (_quizManager.IsFinished)
                            {
                                _quizWaitingForAnswer = false;
                                feedback += $"\n\nQuiz finished! Your score: {_quizManager.GetScore()}/{_quizManager.GetTotalQuestions()}\n{_quizManager.GetFeedback()}";
                                ActivityLogger.Log($"Quiz completed with score {_quizManager.GetScore()}/{_quizManager.GetTotalQuestions()}");
                                return feedback;
                            }

                            // Show next question
                            if (currentQ != null)
                            {
                                string options = "";
                                for (int i = 0; i < currentQ.Options.Count; i++)
                                    options += $"{i + 1}. {currentQ.Options[i]}\n";
                                return feedback + $"\n\nNext question:\n\n{currentQ.Text}\n\n{options}\nType the number of your answer (1-{currentQ.Options.Count}).";
                            }
                            return feedback;
                        }
                        else
                        {
                            return "Please enter the number corresponding to your answer (1, 2, 3, or 4).";
                        }
                    }

                    // Handle task completion/deletion with ID
                    if (intent == NLPHelper.Intent.CompleteTask)
                    {
                        var match = Regex.Match(rawInput, @"\b(\d+)\b");
                        if (match.Success && int.TryParse(match.Groups[1].Value, out int id))
                        {
                            _taskManager.CompleteTask(id);
                            ActivityLogger.Log($"Task {id} completed via chat");
                            return $"Task {id} marked as completed!";
                        }
                        return "Please provide the task ID. Example: 'complete task 3'";
                    }

                    if (intent == NLPHelper.Intent.DeleteTask)
                    {
                        var match = Regex.Match(rawInput, @"\b(\d+)\b");
                        if (match.Success && int.TryParse(match.Groups[1].Value, out int id))
                        {
                            _taskManager.DeleteTask(id);
                            ActivityLogger.Log($"Task {id} deleted via chat");
                            return $"Task {id} deleted!";
                        }
                        return "Please provide the task ID. Example: 'delete task 3'";
                    }

                    // Fall through to existing keyword recognition
                    break;
            }

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

        /// <summary>
        /// Clears the user name and favorite topic from memory.
        /// </summary>
        public void ClearMemory()
        {
            _userName = null;
            _favoriteTopic = null;
            _lastTopic = null;
        }

        public TaskManager GetTaskManager() => _taskManager;
        public QuizManager GetQuizManager() => _quizManager;
        public TaskManager TaskManager => _taskManager;
        public QuizManager QuizManager => _quizManager;

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
