using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Category> GetAllCategories()
        {
            var categories = new List<Category>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM sqlCategories", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        CategoryId = (int)reader["CategoryId"],
                        CategoryName = reader["CategoryName"].ToString(),
                        ParentCategoryId = reader["ParentCategoryId"] as int?,
                        ImageUrl = reader["ImageUrl"]?.ToString(),
                        IsActive = (bool)reader["IsActive"]
                    });
                }
            }
            return categories;
        }

        public Category GetCategoryById(int id)
        {
            Category category = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM sqlCategories WHERE CategoryId = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    category = new Category
                    {
                        CategoryId = (int)reader["CategoryId"],
                        CategoryName = reader["CategoryName"].ToString(),
                        ParentCategoryId = reader["ParentCategoryId"] as int?,
                        ImageUrl = reader["ImageUrl"]?.ToString(),
                        IsActive = (bool)reader["IsActive"]
                    };
                }
            }
            return category;
        }

        public void AddCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO sqlCategories (CategoryName, ParentCategoryId, ImageUrl, IsActive) VALUES (@name, @parent, @image, @active)",
                    conn
                );
                cmd.Parameters.AddWithValue("@name", category.CategoryName);
                cmd.Parameters.AddWithValue("@parent", (object?)category.ParentCategoryId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@image", (object?)category.ImageUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@active", category.IsActive);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE sqlCategories SET CategoryName=@name, ParentCategoryId=@parent, ImageUrl=@image, IsActive=@active WHERE CategoryId=@id",
                    conn
                );
                cmd.Parameters.AddWithValue("@name", category.CategoryName);
                cmd.Parameters.AddWithValue("@parent", (object?)category.ParentCategoryId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@image", (object?)category.ImageUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@active", category.IsActive);
                cmd.Parameters.AddWithValue("@id", category.CategoryId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCategory(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM sqlCategories WHERE CategoryId=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
