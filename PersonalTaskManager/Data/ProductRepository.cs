using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Product> GetProducts(string? search, int? categoryId, string? sortBy, string? sortOrder, int page, int pageSize)
        {
            var products = new List<Product>();
            var offset = (page - 1) * pageSize;

            using (var conn = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Products WHERE 1=1";

                if (!string.IsNullOrEmpty(search))
                    query += " AND ProductName LIKE @search";

                if (categoryId.HasValue)
                    query += " AND CategoryId = @categoryId";

                if (!string.IsNullOrEmpty(sortBy))
                {
                    sortOrder = string.IsNullOrEmpty(sortOrder) ? "ASC" : sortOrder;
                    query += $" ORDER BY {sortBy} {sortOrder}";
                }
                else
                {
                    query += " ORDER BY CreatedDate DESC";
                }

                query += " OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(search))
                        cmd.Parameters.AddWithValue("@search", $"%{search}%");

                    if (categoryId.HasValue)
                        cmd.Parameters.AddWithValue("@categoryId", categoryId.Value);

                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);

                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductId = (int)reader["ProductId"],
                            ProductName = reader["ProductName"].ToString(),
                            Description = reader["Description"]?.ToString(),
                            Price = (decimal)reader["Price"],
                            Stock = (int)reader["Stock"],
                            CategoryId = (int)reader["CategoryId"],
                            ImageUrl = reader["ImageUrl"]?.ToString(),
                            CreatedDate = (DateTime)reader["CreatedDate"],
                            IsActive = (bool)reader["IsActive"]
                        });
                    }
                }
            }
            return products;
        }

        public Product GetProductById(int id)
        {
            Product product = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Products WHERE ProductId=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    product = new Product
                    {
                        ProductId = (int)reader["ProductId"],
                        ProductName = reader["ProductName"].ToString(),
                        Description = reader["Description"]?.ToString(),
                        Price = (decimal)reader["Price"],
                        Stock = (int)reader["Stock"],
                        CategoryId = (int)reader["CategoryId"],
                        ImageUrl = reader["ImageUrl"]?.ToString(),
                        CreatedDate = (DateTime)reader["CreatedDate"],
                        IsActive = (bool)reader["IsActive"]
                    };
                }
            }
            return product;
        }

        public void AddProduct(Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(
                    "INSERT INTO Products (ProductName, Description, Price, Stock, CategoryId, ImageUrl, IsActive) " +
                    "VALUES (@name, @desc, @price, @stock, @catId, @image, @active)", conn);

                cmd.Parameters.AddWithValue("@name", product.ProductName);
                cmd.Parameters.AddWithValue("@desc", (object?)product.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@stock", product.Stock);
                cmd.Parameters.AddWithValue("@catId", product.CategoryId);
                cmd.Parameters.AddWithValue("@image", (object?)product.ImageUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@active", product.IsActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(
                    "UPDATE Products SET ProductName=@name, Description=@desc, Price=@price, Stock=@stock, " +
                    "CategoryId=@catId, ImageUrl=@image, IsActive=@active WHERE ProductId=@id", conn);

                cmd.Parameters.AddWithValue("@name", product.ProductName);
                cmd.Parameters.AddWithValue("@desc", (object?)product.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@stock", product.Stock);
                cmd.Parameters.AddWithValue("@catId", product.CategoryId);
                cmd.Parameters.AddWithValue("@image", (object?)product.ImageUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@active", product.IsActive);
                cmd.Parameters.AddWithValue("@id", product.ProductId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("DELETE FROM Products WHERE ProductId=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
