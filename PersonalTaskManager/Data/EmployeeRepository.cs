using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connStr;
        public EmployeeRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Employee> GetAll()
        {
            var list = new List<Employee>();
            using var conn = new SqlConnection(_connStr);
            using var cmd = new SqlCommand(
                @"SELECT EmployeeId, FirstName, LastName, Email, Phone, DepartmentId, Salary, HireDate, IsActive
                  FROM Employees", conn);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(MapReaderToEmployee(rdr));
            }
            return list;
        }

        public Employee GetById(int id)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = new SqlCommand(
                @"SELECT EmployeeId, FirstName, LastName, Email, Phone, DepartmentId, Salary, HireDate, IsActive
                  FROM Employees WHERE EmployeeId = @id", conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            if (rdr.Read()) return MapReaderToEmployee(rdr);
            return null;
        }

        public int Create(Employee emp)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = new SqlCommand(
                @"INSERT INTO Employees (FirstName, LastName, Email, Phone, DepartmentId, Salary, HireDate, IsActive)
                  OUTPUT INSERTED.EmployeeId
                  VALUES (@fn, @ln, @email, @phone, @dept, @salary, @hire, @isActive)", conn);

            cmd.Parameters.Add("@fn", SqlDbType.NVarChar, 50).Value = emp.FirstName;
            cmd.Parameters.Add("@ln", SqlDbType.NVarChar, 50).Value = emp.LastName;
            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 100).Value = (object)emp.Email ?? DBNull.Value;
            cmd.Parameters.Add("@phone", SqlDbType.NVarChar, 15).Value = (object)emp.Phone ?? DBNull.Value;
            cmd.Parameters.Add("@dept", SqlDbType.Int).Value = (object)emp.DepartmentId ?? DBNull.Value;
            cmd.Parameters.Add("@salary", SqlDbType.Decimal).Value = (object)emp.Salary ?? DBNull.Value;
            cmd.Parameters.Add("@hire", SqlDbType.Date).Value = (object)emp.HireDate ?? DBNull.Value;
            cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = emp.IsActive;
            conn.Open();
            return (int)cmd.ExecuteScalar();
        }

        public bool Update(Employee emp)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = new SqlCommand(
                @"UPDATE Employees SET FirstName=@fn, LastName=@ln, Email=@email, Phone=@phone,
                  DepartmentId=@dept, Salary=@salary, HireDate=@hire, IsActive=@isActive
                  WHERE EmployeeId=@id", conn);

            cmd.Parameters.Add("@fn", SqlDbType.NVarChar, 50).Value = emp.FirstName;
            cmd.Parameters.Add("@ln", SqlDbType.NVarChar, 50).Value = emp.LastName;
            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 100).Value = (object)emp.Email ?? DBNull.Value;
            cmd.Parameters.Add("@phone", SqlDbType.NVarChar, 15).Value = (object)emp.Phone ?? DBNull.Value;
            cmd.Parameters.Add("@dept", SqlDbType.Int).Value = (object)emp.DepartmentId ?? DBNull.Value;
            cmd.Parameters.Add("@salary", SqlDbType.Decimal).Value = (object)emp.Salary ?? DBNull.Value;
            cmd.Parameters.Add("@hire", SqlDbType.Date).Value = (object)emp.HireDate ?? DBNull.Value;
            cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = emp.IsActive;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = emp.EmployeeId;
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = new SqlCommand("DELETE FROM Employees WHERE EmployeeId = @id", conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        private Employee MapReaderToEmployee(SqlDataReader rdr)
        {
            return new Employee
            {
                EmployeeId = (int)rdr["EmployeeId"],
                FirstName = rdr["FirstName"].ToString(),
                LastName = rdr["LastName"].ToString(),
                Email = rdr["Email"] as string,
                Phone = rdr["Phone"] as string,
                DepartmentId = rdr["DepartmentId"] as int?,
                Salary = rdr["Salary"] as decimal?,
                HireDate = rdr["HireDate"] as DateTime?,
                IsActive = (bool)rdr["IsActive"]
            };
        }
    }
}
