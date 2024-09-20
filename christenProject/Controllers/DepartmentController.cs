using christenProject.Model;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using christenProject.Models.DTOs;
using AutoMapper;
using christenProject.Models;
using Microsoft.AspNetCore.Cors;

namespace christenProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors
        ]
    public class DepartmentController(ITIContext context, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public IActionResult DisplayAllDepartments()
        {
            if (context.Department is null)
                return NotFound();
            return Ok(context.Department.Select(e=>new { DeptId = e.Id, DeptName = e.Name,DeptManger = e.ManagerName  }).ToList());
        }
        [HttpGet("EmpNms")]
        public ActionResult<List<DepartmentResponseDTO>> DisplayAllDepartmentsWithEmpoyeeNames()
        {
            if (context.Department is null)
                return NotFound();
            return Ok(context.Department.Include(d => d.Employees).Select(x => new DepartmentResponseDTO
            {
                Id = x.Id,
                DepartmentName = x.Name,
                EmployeeName = x.Employees.Select(x => x.Name).ToList()
            }).ToList());
            //return context.Department.Include(d => d.Employees).Select(x => new DepartmentResponseDTO
            //{
            //    Id = x.Id,
            //    DepartmentName = x.Name,
            //    EmployeeName = x.Employees.Select(x => x.Name).ToList()
            //}).ToList();
        }
        [HttpGet("{id:int}")]
        public IActionResult GetDepartmentById(int id)
        {
            if (context.Department is null)
                return NotFound();

            var data = context.Department.Include(e=>e.Employees).FirstOrDefault(dpt => dpt.Id == id);
            if (data is null)
                return NotFound();
            return Ok(data);
            //return Ok(new DepartmentResponseDTO { Id = data.Id, EmployeeName = data.Employees.Select(x => x.Name).ToList(),DepartmentName = data.Name });
        }
        [HttpGet("{name:alpha}")]
        public IActionResult GetDepartmentByName(string name)
        {
            if (context.Department is null)
                return NotFound();
            var data = context.Department.FirstOrDefault(dpt => dpt.Name.ToLower() == name.ToLower());
            if (data is null)
                return NotFound();
            return Ok(data);
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateDepartment(int id, DepartmentDTOPostPut department)
        {
            if (department is null)
                return BadRequest();
            // Find the employee by the id from the URL
            var mydepartment = context.Department.FirstOrDefault(e => e.Id == id);

            if (mydepartment == null)
                // If the employee is not found, return 404
                return NotFound("Department Not Found");

            //context.Entry(mydepartment).CurrentValues.SetValues(department);
            mapper.Map(department, mydepartment);
            context.Entry<DepartmentModel>(mydepartment).State = EntityState.Modified;
            // var employeeDto = mapper.Map<DepartmentDTO>(mydepartment);
            // Save the changes to the database
            context.SaveChanges();

            // Return 204 No Content (successful update, no response body needed)
            return NoContent();
        }
        [HttpPost]
        public IActionResult AddDepartment(DepartmentDTOPostPut department)
        {
            if (department is null)
                return BadRequest();
            // Map DTO to entity
            var mydept = new DepartmentModel
            {
                Name = department.Name,
                ManagerName = department.MName
            };
            context.Add(mydept);
            context.SaveChangesAsync();
            // Map the saved entity to a response DTO
            var responseDto = new DepartmentResponseDTO
            {
                Id = mydept.Id,
                DepartmentName = mydept.Name,
                EmployeeName = mydept.Employees.Select(x => x.Name).ToList()
            };
          
            return CreatedAtAction(nameof(GetDepartmentById), new { id = mydept.Id }, responseDto);
        }
    }
}