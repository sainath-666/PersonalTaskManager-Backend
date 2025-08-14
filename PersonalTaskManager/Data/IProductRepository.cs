using System.Collections.Generic;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Data
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts(string? search, int? categoryId, string? sortBy, string? sortOrder, int page, int pageSize);
        Product GetProductById(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
    }
}
