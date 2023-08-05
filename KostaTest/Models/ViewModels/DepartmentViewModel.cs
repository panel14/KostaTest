using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KostaTest.Models.ViewModels
{
    public class DepartmentViewModel
    {
        public Guid Id { get; set; }

        public Guid? ParentDepartmentId { get; set; }

        [RegularExpression(@"[A-Z]+[1-9]+", ErrorMessage = "Формат кода: ААА1")]
        [Remote(action: "ValidateCode", controller: "Department", ErrorMessage = "Такой код уже используется")]
        public string? Code { get; set; }

        [Required(ErrorMessage = "Поле имя дожно быть заполнено")]
        [Remote(action: "ValidateName", controller: "Department", ErrorMessage = "Такое имя уже используется")]
        public string Name { get; set; } = null!;

        public Dictionary<string, Guid> DepartmentNames { get; set; } = new Dictionary<string, Guid>();

        public string Action { get; set; } = null!;

        public DepartmentViewModel() 
        {

        }
    }
}
