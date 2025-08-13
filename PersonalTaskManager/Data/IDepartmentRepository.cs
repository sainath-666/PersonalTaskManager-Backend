using System.Collections.Generic;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Data
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAll();
        Department GetById(int id);
        int Create(Department dept);
        bool Update(Department dept);
        bool Delete(int id);
    }
}
