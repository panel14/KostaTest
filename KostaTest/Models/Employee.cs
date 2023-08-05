using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KostaTest.Models
{
    public class Employee
    {
        
        public decimal Id { get; set; }
        public Guid DepartmentId { get; set; }
        public string SurName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? DocSeries { get; set; }
        public string? DocNumber { get; set; }
        public string Position { get; set; } = null!;
    }
}
