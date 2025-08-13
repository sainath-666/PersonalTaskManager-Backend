using System.Data;
using System.Data.SqlClient;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Data
{
    public class TaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<TaskItem> GetAllTasks()
        {
            var tasks = new List<TaskItem>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM sqlTasks", conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tasks.Add(MapTask(reader));
            }
            return tasks;
        }

        public TaskItem? GetTaskById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM sqlTasks WHERE TaskId = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapTask(reader) : null;
        }

        public int CreateTask(TaskItem task)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"INSERT INTO sqlTasks (Title, Description, IsCompleted, DueDate, CreatedDate) OUTPUT INSERTED.TaskId VALUES (@Title, @Description, @IsCompleted, @DueDate, @CreatedDate)", conn);
            cmd.Parameters.AddWithValue("@Title", task.Title);
            cmd.Parameters.AddWithValue("@Description", (object?)task.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
            cmd.Parameters.AddWithValue("@DueDate", (object?)task.DueDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedDate", task.CreatedDate);
            conn.Open();
            return (int)cmd.ExecuteScalar();
        }

        public bool UpdateTask(int id, TaskItem task)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"UPDATE sqlTasks SET Title=@Title, Description=@Description, IsCompleted=@IsCompleted, DueDate=@DueDate WHERE TaskId=@TaskId", conn);
            cmd.Parameters.AddWithValue("@Title", task.Title);
            cmd.Parameters.AddWithValue("@Description", (object?)task.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
            cmd.Parameters.AddWithValue("@DueDate", (object?)task.DueDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TaskId", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool DeleteTask(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("DELETE FROM sqlTasks WHERE TaskId=@TaskId", conn);
            cmd.Parameters.AddWithValue("@TaskId", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        private TaskItem MapTask(IDataRecord record)
        {
            return new TaskItem
            {
                TaskId = record.GetInt32(record.GetOrdinal("TaskId")),
                Title = record.GetString(record.GetOrdinal("Title")),
                Description = record["Description"] == DBNull.Value ? null : (string)record["Description"],
                IsCompleted = record.GetBoolean(record.GetOrdinal("IsCompleted")),
                CreatedDate = record.GetDateTime(record.GetOrdinal("CreatedDate")),
                DueDate = record["DueDate"] == DBNull.Value ? null : (DateTime?)record["DueDate"]
            };
        }
    }
}
