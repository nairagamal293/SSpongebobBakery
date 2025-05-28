using SpongPopBakery.Models;

namespace SpongPopBakery.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task<IEnumerable<Product>> GetProductsByCategory(int categoryId);
        Task<Product> CreateProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int id);
    }
}
