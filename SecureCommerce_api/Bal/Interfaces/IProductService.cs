using SecureCommerce_api.DTOs.Product;

namespace SecureCommerce_api.Bal.Interfaces
{
    public interface IProductService
    {
        Task<IReadOnlyCollection<ProductDto>> GetProductsAsync();
        Task<IReadOnlyCollection<ProductDto>> GetProductsByVendorAsync(Guid vendorId);
        Task<ProductDto?> GetProductByIdAsync(Guid productId);
        Task<ProductDto> CreateProductAsync(Guid vendorId, CreateProductDto model);
        Task<ProductDto?> UpdateProductAsync(Guid productId, Guid vendorId, UpdateProductDto model);
        Task<bool> DeleteProductAsync(Guid productId, Guid vendorId);
    }
}
