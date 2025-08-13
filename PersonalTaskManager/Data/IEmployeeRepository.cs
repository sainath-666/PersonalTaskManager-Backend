using System.Collections.Generic;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Data
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int id);
        int Create(Employee emp);
        bool Update(Employee emp);
        bool Delete(int id);
    }
}
