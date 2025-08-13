using System;

namespace PersonalTaskManager.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? DepartmentId { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? HireDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
