using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CybersecurityChatbotPart2
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddTask(Task task)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            string sql = @"INSERT INTO tasks (title, description, reminder_date) 
                           VALUES (@title, @desc, @reminder)";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@title", task.Title);
            cmd.Parameters.AddWithValue("@desc", task.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@reminder", task.ReminderDate ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public List<Task> GetTasks(bool includeCompleted = false)
        {
            var tasks = new List<Task>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            string sql = includeCompleted ? "SELECT * FROM tasks ORDER BY created_at DESC" 
                                          : "SELECT * FROM tasks WHERE is_completed = FALSE ORDER BY created_at DESC";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tasks.Add(new Task
                {
                    Id = reader.GetInt32("id"),
                    Title = reader.GetString("title"),
                    Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString("description"),
                    ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date")) ? null : reader.GetDateTime("reminder_date"),
                    IsCompleted = reader.GetBoolean("is_completed"),
                    CreatedAt = reader.GetDateTime("created_at")
                });
            }
            return tasks;
        }

        public void DeleteTask(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            string sql = "DELETE FROM tasks WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public void CompleteTask(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            string sql = "UPDATE tasks SET is_completed = TRUE WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
