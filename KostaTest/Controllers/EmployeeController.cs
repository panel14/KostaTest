using KostaTest.Domain.Repositories.Interfaces;
using KostaTest.Models;
using KostaTest.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KostaTest.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        public PartialViewResult AddEmployee()
        {
            Dictionary<string, Guid> depNames = _departmentRepository.GetDepartmentsNames();
            return PartialView("AddEmployee", new EmployeeViewModel() { DepartmentNames = depNames, DateOfBirth = new DateTime(1970, 1, 1), Action = "Добавление" });
        }

        public PartialViewResult ChangeEmployee(decimal id)
        {
            Employee emp = _employeeRepository.GetEmployeeById(id);
            Dictionary<string, Guid> depNames = _departmentRepository.GetDepartmentsNames();

            EmployeeViewModel viewModel = new()
            {
                Id = emp.Id,
                DepartmentId = emp.DepartmentId,
                SurName = emp.SurName,
                FirstName = emp.FirstName,
                Patronymic = emp.Patronymic,
                DateOfBirth = emp.DateOfBirth,
                DocNumber = emp.DocNumber,
                DocSeries = emp.DocSeries,
                Position = emp.Position,
                DepartmentNames = depNames,
                Action = "Изменение"
            };
            return PartialView("AddEmployee", viewModel);
        }

        [HttpPost]
        public IActionResult ChangeEmployee(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Dictionary<string, Guid> depNames = _departmentRepository.GetDepartmentsNames();
                model.DepartmentNames = depNames;
                return PartialView("AddEmployee", model);
            }

            Employee emp = new()
            {
                Id = model.Id,
                DepartmentId = model.DepartmentId,
                SurName = model.SurName,
                FirstName = model.FirstName,
                Patronymic = model.Patronymic,
                DateOfBirth = model.DateOfBirth,
                DocNumber = model.DocNumber,
                DocSeries = model.DocSeries,
                Position = model.Position,
            };

            if (model.Id == 0)
            {
                _employeeRepository.AddEmployee(emp);
                return Content("Сотрудник успешно добавлен");
            }
            else
            {
                _employeeRepository.UpdateEmployee(emp);
                return Content("Сотрудник изменен");
            }
        }

        public string DeleteEmployee(decimal id)
        {
            _employeeRepository.DeleteEmployee(id);
            return "Сотрудник удален";
        }
    }
}
