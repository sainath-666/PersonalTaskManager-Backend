using Microsoft.AspNetCore.Mvc;
using PersonalTaskManager.Data;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskRepository _repository;
        public TasksController(IConfiguration config)
        {
            _repository = new TaskRepository(config.GetConnectionString("DefaultConnection") ?? "");
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetAll()
        {
            return Ok(_repository.GetAllTasks());
        }

        [HttpGet("{id}")]
        public ActionResult<TaskItem> GetById(int id)
        {
            var task = _repository.GetTaskById(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public ActionResult<TaskItem> Create(TaskItem task)
        {
            var id = _repository.CreateTask(task);
            var created = _repository.GetTaskById(id);
            return CreatedAtAction(nameof(GetById), new { id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, TaskItem task)
        {
            if (!_repository.UpdateTask(id, task)) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_repository.DeleteTask(id)) return NotFound();
            return NoContent();
        }
    }
}
