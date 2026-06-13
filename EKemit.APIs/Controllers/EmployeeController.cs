using EKemit.Core.Entities;
using EKemit.Core.Repository.Contract;
using EKemit.Core.Specifications.Employee_Spec;
using Microsoft.AspNetCore.Mvc;

namespace EKemit.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IGenericRepository<Employee> _employeeRep;

        public EmployeeController(IGenericRepository<Employee> employeeRep)
        {
            _employeeRep = employeeRep;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var Spec = new EmployeeWithDepartmentSpecifications();
            var Employees = await _employeeRep.GetAllWithSpecAsync(Spec);
            return Ok(Employees);


        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<Employee>> GetEmployee( int id)
        {
            var Spec = new EmployeeWithDepartmentSpecifications(id);
            var employee = await _employeeRep.GetEntityWithSpecAsync(Spec);
            if (employee is null)
                return NotFound();

            return Ok(employee);
        }
    }
}
