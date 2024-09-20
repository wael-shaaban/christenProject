using System.Text.Json.Serialization;

namespace christenProject.Models.DTOs
{
    public class DepartmentResponseDTO
    {
        public DepartmentResponseDTO()
        {
            EmployeeName = new List<string>();
        }
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        [JsonIgnore]
        public List<string> EmployeeName { get; set; }
    }
}