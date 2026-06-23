using System;
using System.Collections.Generic;

namespace CybersecurityChatbotPart2
{
    public class TaskManager
    {
        private readonly DatabaseHelper _db;

        public TaskManager(string connectionString)
        {
            _db = new DatabaseHelper(connectionString);
        }

        public void AddTask(string title, string? description = null, DateTime? reminder = null)
        {
            var task = new Task
            {
                Title = title,
                Description = description,
                ReminderDate = reminder
            };
            _db.AddTask(task);
            ActivityLogger.Log($"Task added: '{title}'" + (reminder.HasValue ? $" (Reminder set for {reminder.Value.ToShortDateString()})" : ""));
        }

        public List<Task> GetTasks() => _db.GetTasks(false);

        public void DeleteTask(int id)
        {
            _db.DeleteTask(id);
            ActivityLogger.Log($"Task deleted (ID: {id})");
        }

        public void CompleteTask(int id)
        {
            _db.CompleteTask(id);
            ActivityLogger.Log($"Task marked as completed (ID: {id})");
        }
    }
}
