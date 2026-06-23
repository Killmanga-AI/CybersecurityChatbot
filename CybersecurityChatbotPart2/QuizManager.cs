using System;
using System.Collections.Generic;

namespace CybersecurityChatbotPart2
{
    public class QuizManager
    {
        public class Question
        {
            public string Text { get; set; } = string.Empty;
            public List<string> Options { get; set; } = new List<string>();
            public int CorrectIndex { get; set; }
            public string Explanation { get; set; } = string.Empty;
        }

        private readonly List<Question> _questions;
        private int _currentIndex = -1;
        private int _score = 0;
        private bool _quizActive = false;

        public QuizManager()
        {
            _questions = new List<Question>
            {
                new Question
                {
                    Text = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report it as phishing", "Ignore it" },
                    CorrectIndex = 2,
                    Explanation = "Reporting phishing emails helps prevent scams and protects others."
                },
                new Question
                {
                    Text = "True or False: Using the same password for multiple accounts is safe.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Reusing passwords increases risk; use unique passwords for each account."
                },
                new Question
                {
                    Text = "Which of the following is a strong password?",
                    Options = new List<string> { "123456", "password", "P@ssw0rd!2026", "qwerty" },
                    CorrectIndex = 2,
                    Explanation = "A strong password mixes uppercase, lowercase, numbers, and symbols."
                },
                new Question
                {
                    Text = "What is phishing?",
                    Options = new List<string> { "A type of virus", "A scam to steal personal info", "A social media platform", "A programming language" },
                    CorrectIndex = 1,
                    Explanation = "Phishing is a fraudulent attempt to obtain sensitive data by disguising as a trustworthy entity."
                },
                new Question
                {
                    Text = "True or False: You should enable two-factor authentication (2FA) on your accounts.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 0,
                    Explanation = "2FA adds an extra layer of security beyond just a password."
                },
                new Question
                {
                    Text = "Which is a safe browsing habit?",
                    Options = new List<string> { "Clicking on pop-up ads", "Using public Wi-Fi without VPN", "Checking the URL before entering personal info", "Downloading from untrusted sites" },
                    CorrectIndex = 2,
                    Explanation = "Always verify the website's URL before entering any personal information."
                },
                new Question
                {
                    Text = "What should you do if you think your password has been compromised?",
                    Options = new List<string> { "Change it immediately", "Ignore it", "Share it with friends", "Use the same password" },
                    CorrectIndex = 0,
                    Explanation = "Immediately change your password to prevent unauthorized access."
                },
                new Question
                {
                    Text = "True or False: Social engineering attacks rely on technical hacking skills.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Social engineering manipulates people, not systems, often through deception."
                },
                new Question
                {
                    Text = "What is malware?",
                    Options = new List<string> { "Hardware that is broken", "Software designed to harm or exploit a device", "A type of browser", "A security certificate" },
                    CorrectIndex = 1,
                    Explanation = "Malware includes viruses, ransomware, spyware, and other malicious software."
                },
                new Question
                {
                    Text = "True or False: It's safe to open attachments from unknown senders.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Never open attachments from unknown or untrusted sources as they may contain malware."
                },
                new Question
                {
                    Text = "Which of these is a good practice to secure your online accounts?",
                    Options = new List<string> { "Use your birthday as a password", "Share passwords with family", "Use a password manager", "Write passwords on sticky notes" },
                    CorrectIndex = 2,
                    Explanation = "Password managers generate and store strong unique passwords securely."
                }
            };
        }

        public void StartQuiz()
        {
            _currentIndex = 0;
            _score = 0;
            _quizActive = true;
            ActivityLogger.Log("Quiz started.");
        }

        public bool IsActive => _quizActive;
        public bool IsFinished => !_quizActive && _currentIndex >= _questions.Count;

        public Question? GetCurrentQuestion()
        {
            if (_currentIndex < 0 || _currentIndex >= _questions.Count)
                return null;
            return _questions[_currentIndex];
        }

        public bool AnswerCurrent(int selectedIndex)
        {
            if (!_quizActive || _currentIndex < 0 || _currentIndex >= _questions.Count)
                return false;

            var q = _questions[_currentIndex];
            bool correct = (selectedIndex == q.CorrectIndex);
            if (correct) _score++;
            _currentIndex++;
            if (_currentIndex >= _questions.Count)
            {
                _quizActive = false;
                ActivityLogger.Log($"Quiz completed. Score: {_score}/{_questions.Count}");
            }
            return correct;
        }

        public int GetScore() => _score;
        public int GetTotalQuestions() => _questions.Count;

        public string GetFeedback()
        {
            double pct = (double)_score / _questions.Count;
            if (pct >= 0.8) return "Great job! You're a cybersecurity pro!";
            if (pct >= 0.5) return "Good effort! Keep learning to stay safe online.";
            return "Consider reviewing the cybersecurity basics.";
        }
    }
}
