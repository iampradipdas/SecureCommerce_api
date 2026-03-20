using Microsoft.EntityFrameworkCore;
using SecureCommerce_api.Dal.Entities;
using SecureCommerce_api.Dal.Repositories.Interfaces;

namespace SecureCommerce_api.Dal.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SecureCommerceContext _context;

        public ProductRepository(SecureCommerceContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<Product>> GetProductsAsync(Guid? categoryId = null, string? searchTerm = null)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                .AsNoTracking();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerSearchTerm = searchTerm.ToLower();
                query = query.Where(p => p.Name != null && p.Name.ToLower().Contains(lowerSearchTerm) || 
                                         p.Description != null && p.Description.ToLower().Contains(lowerSearchTerm));
            }

            return await query
                .OrderBy(product => product.Name)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Product>> GetProductsByVendorAsync(Guid vendorId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                .AsNoTracking()
                .Where(product => product.VendorId == vendorId)
                .OrderBy(product => product.Name)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid productId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(product => product.Id == productId);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
