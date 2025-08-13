using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalTaskManager.Data;
using PersonalTaskManager.Models;
using System;

namespace PersonalTaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // requires JWT; remove if you want public endpoints
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepository _repo;
        public DepartmentsController(IDepartmentRepository repo) => _repo = repo;

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var dept = _repo.GetById(id);
            if (dept == null) return NotFound();
            return Ok(dept);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Department dept)
        {
            if (dept == null || string.IsNullOrWhiteSpace(dept.DepartmentName))
                return BadRequest("DepartmentName is required.");

            var newId = _repo.Create(dept);
            return CreatedAtAction(nameof(Get), new { id = newId }, new { DepartmentId = newId });
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Department dept)
        {
            if (dept == null || id != dept.DepartmentId) return BadRequest();
            var existing = _repo.GetById(id);
            if (existing == null) return NotFound();
            var ok = _repo.Update(dept);
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
