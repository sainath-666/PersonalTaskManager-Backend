using Microsoft.AspNetCore.Mvc;
using PersonalTaskManager.Data;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepo;

        public ProductsController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? search, [FromQuery] int? categoryId,
                                    [FromQuery] string? sortBy, [FromQuery] string? sortOrder,
                                    [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(_productRepo.GetProducts(search, categoryId, sortBy, sortOrder, page, pageSize));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productRepo.GetProductById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            _productRepo.AddProduct(product);
            return Ok(new { message = "Product added successfully" });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Product product)
        {
            product.ProductId = id;
            _productRepo.UpdateProduct(product);
            return Ok(new { message = "Product updated successfully" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productRepo.DeleteProduct(id);
            return Ok(new { message = "Product deleted successfully" });
        }
    }
}
