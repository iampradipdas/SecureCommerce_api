using AutoMapper;
using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.Dal.Entities;
using SecureCommerce_api.Dal.Repositories.Interfaces;
using SecureCommerce_api.DTOs.Cart;

namespace SecureCommerce_api.Bal
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto> GetCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId) ?? await CreateCartAsync(userId);
            return MapCart(cart);
        }

        public async Task<(bool Success, string Message, CartDto? Cart)> AddItemAsync(Guid userId, AddCartItemDto model)
        {
            var product = await _cartRepository.GetProductByIdAsync(model.ProductId);
            if (product == null)
            {
                return (false, "Product not found.", null);
            }

            var availableStock = product.Stock ?? 0;
            if (availableStock < model.Quantity)
            {
                return (false, "Requested quantity exceeds available stock.", null);
            }

            var cart = await _cartRepository.GetCartByUserIdAsync(userId) ?? await CreateCartAsync(userId);
            var existingItem = cart.CartItems.FirstOrDefault(item => item.ProductId == model.ProductId);

            if (existingItem == null)
            {
                existingItem = new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    ProductId = model.ProductId,
                    Quantity = model.Quantity,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _cartRepository.AddCartItemAsync(existingItem);
            }
            else
            {
                var updatedQuantity = existingItem.Quantity + model.Quantity;
                if (availableStock < updatedQuantity)
                {
                    return (false, "Requested quantity exceeds available stock.", null);
                }

                existingItem.Quantity = updatedQuantity;
                existingItem.UpdatedAt = DateTime.UtcNow;
                await _cartRepository.UpdateCartItemAsync(existingItem);
            }

            var updatedCart = await _cartRepository.GetCartByUserIdAsync(userId) ?? cart;
            return (true, "Item added to cart.", MapCart(updatedCart));
        }

        public async Task<(bool Success, string Message, CartDto? Cart)> UpdateItemAsync(Guid userId, Guid productId, UpdateCartItemDto model)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return (false, "Cart not found.", null);
            }

            var cartItem = cart.CartItems.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem == null)
            {
                return (false, "Cart item not found.", null);
            }

            var product = cartItem.Product ?? await _cartRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                return (false, "Product not found.", null);
            }

            var availableStock = product.Stock ?? 0;
            if (availableStock < model.Quantity)
            {
                return (false, "Requested quantity exceeds available stock.", null);
            }

            cartItem.Quantity = model.Quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateCartItemAsync(cartItem);

            var updatedCart = await _cartRepository.GetCartByUserIdAsync(userId) ?? cart;
            return (true, "Cart item updated.", MapCart(updatedCart));
        }

        public async Task<(bool Success, string Message, CartDto? Cart)> RemoveItemAsync(Guid userId, Guid productId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return (false, "Cart not found.", null);
            }

            var cartItem = cart.CartItems.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem == null)
            {
                return (false, "Cart item not found.", null);
            }

            await _cartRepository.DeleteCartItemAsync(cartItem);

            var updatedCart = await _cartRepository.GetCartByUserIdAsync(userId) ?? await CreateCartAsync(userId);
            return (true, "Cart item removed.", MapCart(updatedCart));
        }

        public async Task<(bool Success, string Message)> ClearCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
            {
                return (true, "Cart is already empty.");
            }

            await _cartRepository.ClearCartItemsAsync(cart.Id);
            return (true, "Cart cleared successfully.");
        }

        private async Task<Cart> CreateCartAsync(Guid userId)
        {
            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await _cartRepository.CreateCartAsync(cart);
        }

        private CartDto MapCart(Cart cart)
        {
            var cartDto = _mapper.Map<CartDto>(cart);
            cartDto.TotalItems = cartDto.Items.Sum(item => item.Quantity);
            cartDto.TotalAmount = cartDto.Items.Sum(item => item.LineTotal);
            return cartDto;
        }
    }
}
