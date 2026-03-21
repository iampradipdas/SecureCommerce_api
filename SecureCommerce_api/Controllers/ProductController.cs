using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.DTOs.Product;

namespace SecureCommerce_api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] Guid? categoryId, [FromQuery] string? searchTerm)
        {
            var products = await _productService.GetProductsAsync(categoryId, searchTerm);
            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { Message = "Product not found." });
            }

            return Ok(product);
        }

        [Authorize]
        [HttpGet("my-products")]
        public async Task<IActionResult> GetMyProducts()
        {
            var vendorId = GetCurrentUserId();
            if (vendorId == null)
            {
                return Unauthorized(new { Message = "Invalid user token." });
            }

            var products = await _productService.GetProductsByVendorAsync(vendorId.Value);
            return Ok(products);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vendorId = GetCurrentUserId();
            if (vendorId == null)
            {
                return Unauthorized(new { Message = "Invalid user token." });
            }

            var product = await _productService.CreateProductAsync(vendorId.Value, model);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vendorId = GetCurrentUserId();
            if (vendorId == null)
            {
                return Unauthorized(new { Message = "Invalid user token." });
            }

            var product = await _productService.UpdateProductAsync(id, vendorId.Value, model);
            if (product == null)
            {
                return NotFound(new { Message = "Product not found for this vendor." });
            }

            return Ok(product);
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var vendorId = GetCurrentUserId();
            if (vendorId == null)
            {
                return Unauthorized(new { Message = "Invalid user token." });
            }

            var deleted = await _productService.DeleteProductAsync(id, vendorId.Value);
            if (!deleted)
            {
                return NotFound(new { Message = "Product not found for this vendor." });
            }

            return NoContent();
        }

        private Guid? GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.NameId);

            return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : null;
        }
    }
}
