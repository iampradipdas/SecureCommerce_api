using SecureCommerce_api.Dal.Entities;

namespace SecureCommerce_api.Dal.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IReadOnlyCollection<Product>> GetProductsAsync(Guid? categoryId = null, string? searchTerm = null);
        Task<IReadOnlyCollection<Product>> GetProductsByVendorAsync(Guid vendorId);
        Task<Product?> GetProductByIdAsync(Guid productId);
        Task<Product> CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
    }
}
