using christenProject.Models;

namespace christenProject.Model
{
    public class DepartmentModel
    {
        public DepartmentModel()
        {
            Employees = new HashSet<EmployeeModel>();
        }
        public int Id  { get; set; }
        public string Name { get; set; }
        public string? ManagerName { get; set; }
        public ICollection<EmployeeModel>?  Employees { get; set; }
    }
}
