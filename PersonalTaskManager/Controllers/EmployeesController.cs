using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalTaskManager.Data;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repo;
        public EmployeesController(IEmployeeRepository repo) => _repo = repo;

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var emp = _repo.GetById(id);
            if (emp == null) return NotFound();
            return Ok(emp);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Employee emp)
        {
            if (emp == null || string.IsNullOrWhiteSpace(emp.FirstName) || string.IsNullOrWhiteSpace(emp.LastName))
                return BadRequest("FirstName and LastName are required.");

            var newId = _repo.Create(emp);
            return CreatedAtAction(nameof(Get), new { id = newId }, new { EmployeeId = newId });
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Employee emp)
        {
            if (emp == null || id != emp.EmployeeId) return BadRequest();
            var existing = _repo.GetById(id);
            if (existing == null) return NotFound();

            var ok = _repo.Update(emp);
            if (!ok) return StatusCode(500, "Could not update.");
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var existing = _repo.GetById(id);
            if (existing == null) return NotFound();
            var ok = _repo.Delete(id);
            if (!ok) return StatusCode(500, "Could not delete.");
            return NoContent();
        }
    }
}
