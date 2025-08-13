using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Data
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly string _connStr;
        public DepartmentRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Department> GetAll()
        {
            var list = new List<Department>();
            using var conn = new SqlConnection(_connStr);
            using var cmd = new SqlCommand("SELECT DepartmentId, DepartmentName, Description FROM Departments", conn);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new Department
                {
                    DepartmentId = (int)rdr["DepartmentId"],
                    DepartmentName = rdr["DepartmentName"].ToString(),
                    Description = rdr["Description"] as string
                });
            }
            return list;
        }

        public Department GetById(int id)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = new SqlCommand("SELECT DepartmentId, DepartmentName, Description FROM Departments WHERE DepartmentId = @id", conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                return new Department
                {
                    DepartmentId = (int)rdr["DepartmentId"],
                    DepartmentName = rdr["DepartmentName"].ToString(),
                    Description = rdr["Description"] as string
                };
            }
            return null;
        }

        public int Create(Department dept)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = new SqlCommand("INSERT INTO Departments (DepartmentName, Description) OUTPUT INSERTED.DepartmentId VALUES (@name, @desc)", conn);
            cmd.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = dept.DepartmentName;
            cmd.Parameters.Add("@desc", SqlDbType.NVarChar, 200).Value = (object)dept.Description ?? DBNull.Value;
            conn.Open();
            return (int)cmd.ExecuteScalar();
        }

        public bool Update(Department dept)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = new SqlCommand("UPDATE Departments SET DepartmentName = @name, Description = @desc WHERE DepartmentId = @id", conn);
            cmd.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = dept.DepartmentName;
            cmd.Parameters.Add("@desc", SqlDbType.NVarChar, 200).Value = (object)dept.Description ?? DBNull.Value;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = dept.DepartmentId;
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = new SqlCommand("DELETE FROM Departments WHERE DepartmentId = @id", conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
