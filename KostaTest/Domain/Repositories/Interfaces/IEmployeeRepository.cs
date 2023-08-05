using KostaTest.Models;

namespace KostaTest.Domain.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        public List<Employee> GetEmployeesByDepartmentId(Guid departmentId);

        public Employee GetEmployeeById(decimal id);
        public void AddEmployee(Employee emp);
        public void UpdateEmployee(Employee emp);
        public void DeleteEmployee(decimal id);
    }
}
