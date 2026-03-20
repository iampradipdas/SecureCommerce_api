using AutoMapper;
using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.Dal.Entities;
using SecureCommerce_api.Dal.Repositories.Interfaces;
using SecureCommerce_api.DTOs.Product;

namespace SecureCommerce_api.Bal
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<ProductDto>> GetProductsAsync(Guid? categoryId = null, string? searchTerm = null)
        {
            var products = await _productRepository.GetProductsAsync(categoryId, searchTerm);
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<IReadOnlyCollection<ProductDto>> GetProductsByVendorAsync(Guid vendorId)
        {
            var products = await _productRepository.GetProductsByVendorAsync(vendorId);
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateProductAsync(Guid vendorId, CreateProductDto model)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = model.Name.Trim(),
                Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
                Price = model.Price,
                Stock = model.Stock,
                VendorId = vendorId,
                CategoryId = model.CategoryId,
                ImageUrl = model.ImageUrl
            };

            var createdProduct = await _productRepository.CreateProductAsync(product);
            return _mapper.Map<ProductDto>(createdProduct);
        }

        public async Task<ProductDto?> UpdateProductAsync(Guid productId, Guid vendorId, UpdateProductDto model)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null || product.VendorId != vendorId)
            {
                return null;
            }

            product.Name = model.Name.Trim();
            product.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.CategoryId = model.CategoryId;
            product.ImageUrl = model.ImageUrl;

            await _productRepository.UpdateProductAsync(product);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> DeleteProductAsync(Guid productId, Guid vendorId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null || product.VendorId != vendorId)
            {
                return false;
            }

            await _productRepository.DeleteProductAsync(product);
            return true;
        }
    }
}
