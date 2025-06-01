using Microsoft.EntityFrameworkCore;
using SpongPopBakery.Data;
using SpongPopBakery.Models;

namespace SpongPopBakery.Services
{
    public class ProductService : IProductService
    {
        private readonly BakeryDbContext _context;

        public ProductService(BakeryDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            

            // Instead, just ensure at least one size is marked as default
            if (product.Sizes.Any() && !product.Sizes.Any(s => s.IsDefault))
            {
                product.Sizes.First().IsDefault = true;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Sizes)
                .ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Sizes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .Include(p => p.Sizes)
                .ToListAsync();
        }


        public async Task UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
