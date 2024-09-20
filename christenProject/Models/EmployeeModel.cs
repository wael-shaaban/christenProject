using christenProject.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace christenProject.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }
        public DepartmentModel? Department { get; set; }
    }
}
