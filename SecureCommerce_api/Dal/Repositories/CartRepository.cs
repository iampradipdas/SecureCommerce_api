using Microsoft.EntityFrameworkCore;
using SecureCommerce_api.Dal.Entities;
using SecureCommerce_api.Dal.Repositories.Interfaces;

namespace SecureCommerce_api.Dal.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly SecureCommerceContext _context;

        public CartRepository(SecureCommerceContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
        {
            return await _context.Carts
                .Include(cart => cart.CartItems)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(cart => cart.UserId == userId);
        }

        public async Task<Cart> CreateCartAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Product?> GetProductByIdAsync(Guid productId)
        {
            return await _context.Products.FirstOrDefaultAsync(product => product.Id == productId);
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);

            var cart = await _context.Carts.FirstAsync(existingCart => existingCart.Id == cartItem.CartId);
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);

            var cart = await _context.Carts.FirstAsync(existingCart => existingCart.Id == cartItem.CartId);
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);

            var cart = await _context.Carts.FirstAsync(existingCart => existingCart.Id == cartItem.CartId);
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task ClearCartItemsAsync(Guid cartId)
        {
            var cartItems = await _context.CartItems.Where(item => item.CartId == cartId).ToListAsync();
            _context.CartItems.RemoveRange(cartItems);

            var cart = await _context.Carts.FirstAsync(existingCart => existingCart.Id == cartId);
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
