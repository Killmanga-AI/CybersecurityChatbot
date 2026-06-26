using System;
using System.Windows;

namespace CybersecurityChatbotPart2
{
    public partial class TaskWindow : Window
    {
        private TaskManager _taskManager;
        private Task? _selectedTask;

        public TaskWindow(TaskManager taskManager)
        {
            InitializeComponent();
            _taskManager = taskManager;
            RefreshTasks();
        }

        private void RefreshTasks()
        {
            var tasks = _taskManager.GetTasks();
            LstTasks.ItemsSource = null;
            LstTasks.ItemsSource = tasks;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string title = TxtTitle.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a task title.");
                return;
            }
            string desc = TxtDesc.Text.Trim();
            DateTime? reminder = DpReminder.SelectedDate;
            _taskManager.AddTask(title, string.IsNullOrEmpty(desc) ? null : desc, reminder);
            RefreshTasks();
            TxtTitle.Clear();
            TxtDesc.Clear();
            DpReminder.SelectedDate = null;
        }

        private void LstTasks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _selectedTask = LstTasks.SelectedItem as Task;
        }

        private void BtnComplete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTask == null) return;
            _taskManager.CompleteTask(_selectedTask.Id);
            RefreshTasks();
            _selectedTask = null;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTask == null) return;
            if (MessageBox.Show($"Delete task '{_selectedTask.Title}'?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _taskManager.DeleteTask(_selectedTask.Id);
                RefreshTasks();
                _selectedTask = null;
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTasks();
        }
    }
}
