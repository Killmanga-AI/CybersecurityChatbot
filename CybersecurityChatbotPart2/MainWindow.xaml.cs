using System;
using System.Collections.ObjectModel;
using System.Media;
using System.Windows;
using System.Windows.Input;

namespace CybersecurityChatbotPart2
{
    public partial class MainWindow : Window
    {
        private ChatBotEngine _bot;
        private SoundPlayer? _greetingPlayer;
        public ObservableCollection<string> ChatMessages { get; set; } = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            _bot = new ChatBotEngine();
            ChatHistoryBox.ItemsSource = ChatMessages;

            AsciiArtBlock.Text = @"
 ██████╗ ██████╗ ██████╗ ██╗████████╗
██╔═══██╗██╔══██╗██╔══██╗██║╚══██╔══╝
██║   ██║██████╔╝██████╔╝██║   ██║   
██║   ██║██╔══██╗██╔══██╗██║   ██║   
╚██████╔╝██║  ██║██████╔╝██║   ██║   
 ╚═════╝ ╚═╝  ╚═╝╚═════╝ ╚═╝   ╚═╝   ";

            try
            {
                _greetingPlayer = new SoundPlayer("greeting.wav");
                _greetingPlayer.Play();
            }
            catch (Exception ex)
            {
                AddBotMessage($"[Error] Voice not found: {ex.Message}");
            }

            AddBotMessage("Hello! I am your Orbit Security assistant. How can I help you?");
            InputTextBox.Focus();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e) => ProcessInput();
        
        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ProcessInput();
        }

        private void ProcessInput()
        {
            string inputText = InputTextBox.Text.Trim();
            if (string.IsNullOrEmpty(inputText)) return;

            AddUserMessage(inputText);
            InputTextBox.Clear();
            
            string response = _bot.GetResponse(inputText);
            AddBotMessage(response);
        }

        private void AddUserMessage(string message)
        {
            ChatMessages.Add($"You: {message}");
            ScrollToBottom();
        }

        private void AddBotMessage(string message)
        {
            ChatMessages.Add($"Bot: {message}");
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            if (ChatHistoryBox.Items.Count > 0)
            {
                var border = System.Windows.Media.VisualTreeHelper.GetChild(ChatHistoryBox, 0) as System.Windows.Controls.Border;
                var scrollViewer = border?.Child as System.Windows.Controls.ScrollViewer;
                scrollViewer?.ScrollToBottom();
            }
        }
    }
}
