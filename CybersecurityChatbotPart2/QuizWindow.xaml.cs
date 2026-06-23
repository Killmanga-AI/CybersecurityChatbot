using System;
using System.Windows;
using System.Windows.Controls;

namespace CybersecurityChatbotPart2
{
    public partial class QuizWindow : Window
    {
        private QuizManager _quiz;
        private int _selectedOption = -1;
        private bool _answered = false;

        public QuizWindow(QuizManager quiz)
        {
            InitializeComponent();
            _quiz = quiz;
            _quiz.StartQuiz();
            DisplayQuestion();
        }

        private void DisplayQuestion()
        {
            var q = _quiz.GetCurrentQuestion();
            if (q == null)
            {
                ShowFinished();
                return;
            }

            TblQuestionNumber.Text = $"Question {_quiz.GetScore() + 1} of {_quiz.GetTotalQuestions()}";
            TblQuestion.Text = q.Text;
            OptionsPanel.Children.Clear();
            _selectedOption = -1;
            _answered = false;

            for (int i = 0; i < q.Options.Count; i++)
            {
                var btn = new RadioButton
                {
                    Content = $"{i + 1}. {q.Options[i]}",
                    Tag = i,
                    GroupName = "Options",
                    Margin = new Thickness(5)
                };
                btn.Checked += (s, e) => _selectedOption = (int)((RadioButton)s).Tag;
                OptionsPanel.Children.Add(btn);
            }

            TblFeedback.Text = "";
            BtnNext.Content = "Submit";
            TblScore.Text = $"Score: {_quiz.GetScore()}";
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (!_answered)
            {
                // Submit answer
                if (_selectedOption == -1)
                {
                    MessageBox.Show("Please select an answer.");
                    return;
                }

                bool correct = _quiz.AnswerCurrent(_selectedOption);
                var q = _quiz.GetCurrentQuestion();

                TblFeedback.Text = correct ? "Correct! " : "Incorrect. ";
                TblFeedback.Text += q?.Explanation ?? "";
                _answered = true;
                BtnNext.Content = "Next";

                if (_quiz.IsFinished)
                {
                    ShowFinished();
                    return;
                }
            }
            else
            {
                // Move to next question
                DisplayQuestion();
            }
        }

        private void ShowFinished()
        {
            TblQuestionNumber.Text = "Quiz Complete!";
            TblQuestion.Text = $"Final Score: {_quiz.GetScore()} out of {_quiz.GetTotalQuestions()}";
            TblQuestion.Text += $"\n\n{_quiz.GetFeedback()}";
            OptionsPanel.Children.Clear();
            TblFeedback.Text = "";
            BtnNext.Content = "Close";
            BtnNext.Click -= BtnNext_Click;
            BtnNext.Click += (s, e) => Close();
            TblScore.Text = $"Final Score: {_quiz.GetScore()}";
        }
    }
}
