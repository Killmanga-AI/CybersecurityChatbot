using System.Windows;

namespace CybersecurityChatbotPart2
{
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            try
            {
                InitializeComponent();
                RefreshLog();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error opening Log window:\n{ex.Message}", "Log Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void RefreshLog()
        {
            var log = ActivityLogger.GetLog(20);
            LstLog.ItemsSource = null;
            LstLog.ItemsSource = log;

            if (log.Count == 0)
                TxtEmpty.Visibility = Visibility.Visible;
            else
                TxtEmpty.Visibility = Visibility.Collapsed;
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
