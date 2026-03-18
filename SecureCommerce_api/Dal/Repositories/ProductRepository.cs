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

        public async Task<IReadOnlyCollection<Product>> GetProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .OrderBy(product => product.Name)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Product>> GetProductsByVendorAsync(Guid vendorId)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(product => product.VendorId == vendorId)
                .OrderBy(product => product.Name)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid productId)
        {
            return await _context.Products
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
