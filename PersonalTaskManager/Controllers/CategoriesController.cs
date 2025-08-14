using Microsoft.AspNetCore.Mvc;
using PersonalTaskManager.Data;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoriesController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_categoryRepo.GetAllCategories());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _categoryRepo.GetCategoryById(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            _categoryRepo.AddCategory(category);
            return Ok(new { message = "Category added successfully" });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Category category)
        {
            category.CategoryId = id;
            _categoryRepo.UpdateCategory(category);
            return Ok(new { message = "Category updated successfully" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _categoryRepo.DeleteCategory(id);
            return Ok(new { message = "Category deleted successfully" });
        }
    }
}
