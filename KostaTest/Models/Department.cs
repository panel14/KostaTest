namespace KostaTest.Models
{
    public class Department
    {
        public Guid Id { get; set; }
        public Guid? ParentDepartmentId { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; } = null!;
    }
}
