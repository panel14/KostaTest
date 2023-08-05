using KostaTest.Domain.Repositories.Interfaces;
using KostaTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace KostaTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public HomeController(IDepartmentRepository departmentRepository, IEmployeeRepository employeeRepository)
        {
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;
        }

        public IActionResult Index()
        {
            List<Department> departments = _departmentRepository.GetAllDepartments();
            return View(departments);
        }

        public IActionResult ViewEmployes(Guid id)
        {
            List<Employee> employees = _employeeRepository.GetEmployeesByDepartmentId(id);
            ViewBag.DepName = _departmentRepository.GetDepartmentById(id).Name;
            return PartialView(employees);
        }
    }
}