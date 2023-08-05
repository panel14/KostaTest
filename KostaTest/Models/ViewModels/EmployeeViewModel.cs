using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KostaTest.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public decimal Id { get; set; }

        public Guid DepartmentId { get; set; }

        [Required(ErrorMessage = "Не указана фамилия сотрудника")]
        [RegularExpression(@"[A-Za-zА-Яа-я]+", ErrorMessage = "Фамилия может содержать только буквы")]
        public string SurName { get; set; } = null!;

        [Required(ErrorMessage = "Не указано имя сотрудника")]
        [RegularExpression(@"[A-Za-zА-Яа-я]+", ErrorMessage = "Имя может содержать только буквы")]
        public string FirstName { get; set; } = null!;

        [RegularExpression(@"[A-Za-zА-Яа-я]+", ErrorMessage = "Отчество может содержать только буквы")]
        public string? Patronymic { get; set; }

        public DateTime DateOfBirth { get; set; }

        [RegularExpression(@"[A-Z0-9]{4}", ErrorMessage = "Серия документа содержит 4 латинские буквы или цифры")]
        public string? DocSeries { get; set; }

        [RegularExpression(@"[0-9]{6}", ErrorMessage = "Номер документа состоит из 6 цифр")]
        public string? DocNumber { get; set; }

        [RegularExpression(@"[A-Za-zА-Яа-я\s]+", ErrorMessage = "Позиция может содержать только буквы")]
        public string Position { get; set; } = null!;

        public Dictionary<string, Guid> DepartmentNames { get; set; } = new Dictionary<string, Guid>();

        public string Action { get; set; } = null!;

        public EmployeeViewModel() { }

    }
}
