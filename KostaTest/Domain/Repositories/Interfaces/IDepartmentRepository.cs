using KostaTest.Models;

namespace KostaTest.Domain.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        public List<Department> GetAllDepartments();
        public Department GetDepartmentById(Guid id);
        public Dictionary<string, Guid> GetDepartmentsNames();
        public void AddDepartment(Department dep);
        public void UpdateDepartment(Department dep);
        public void DeleteDepartment(Guid id);

    }
}
