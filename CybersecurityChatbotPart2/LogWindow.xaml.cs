using System.Windows;

namespace CybersecurityChatbotPart2
{
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();
            RefreshLog();
        }

        private void RefreshLog()
        {
            var log = ActivityLogger.GetLog(20);
            LstLog.ItemsSource = log;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshLog();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ActivityLogger.Clear();
            RefreshLog();
        }
    }
}
