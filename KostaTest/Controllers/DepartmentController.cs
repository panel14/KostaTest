using KostaTest.Domain.Repositories.Interfaces;
using KostaTest.Models;
using KostaTest.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KostaTest.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController(IDepartmentRepository departmentRepository) 
        {
            _departmentRepository = departmentRepository;
        }
        public PartialViewResult AddDepartment()
        {
            Dictionary<string, Guid> depNames = _departmentRepository.GetDepartmentsNames();
            depNames.Add("--нет--", Guid.Empty);
            return PartialView("AddDepartment", new DepartmentViewModel() { DepartmentNames = depNames, Action = "Добавление"});
        }

        public PartialViewResult ChangeDepartment(Guid id) 
        {
            Department department = _departmentRepository.GetDepartmentById(id);
            Dictionary<string, Guid> depNames = _departmentRepository.GetDepartmentsNames();

            DepartmentViewModel viewModel = new()
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code,
                ParentDepartmentId = department.ParentDepartmentId,
                DepartmentNames = depNames,
                Action = "Изменение"
            };
            return PartialView("AddDepartment", viewModel);
        }

        [HttpPost]
        public IActionResult ChangeDepartment(DepartmentViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                Dictionary<string, Guid> depNames = _departmentRepository.GetDepartmentsNames();
                model.DepartmentNames = depNames;
                return PartialView("AddDepartment", model);
            }

            Department dep = new()
            {
                Id = model.Id,
                Name = model.Name,
                Code = model.Code,
                ParentDepartmentId = model.ParentDepartmentId
            };


            List<Department> deps = _departmentRepository.GetAllDepartments();
            if (deps.Select(x => x.Name).FirstOrDefault(x => x == model.Name) != null)
            {
                return Content($"Отдел '{model.Name}' не может быть добавлен, так как отдел с таким именем уже существует");
            }

            if (deps.Select(x => x.Code).FirstOrDefault(x => x == model.Code) != null)
            {
                return Content($"Отдел '{model.Name}' не может быть добавлен, так как отдел с таким кодом ({model.Code}) уже существует");
            }

            if (dep.Id == Guid.Empty)
            {
                _departmentRepository.AddDepartment(dep);
                return Content($"Новый отдел '{model.Name}' успешно добавлен");
            }
            else
            {
                _departmentRepository.UpdateDepartment(dep);
                return Content($"Отдел '{model.Name}' успешно изменен");
            }
        }

        public string DeleteDepartment(Guid id)
        {
            _departmentRepository.DeleteDepartment(id);
            return "Отдел удален";
        }

        public IActionResult ValidateName(string name)
        {
            List<string> names = _departmentRepository.GetDepartmentsNames().Keys.ToList();
            if (names.Contains(name)) 
            {
                return Json(false);
            }
            return Json(true);
        }

        public IActionResult ValidateCode(string code)
        {
            List<string?> codes = _departmentRepository.GetAllDepartments().Select(x => x.Code).ToList();
            if (codes.Contains(code))
            {
                return Json(false);
            }
            return Json(true);
        }
    }
}
