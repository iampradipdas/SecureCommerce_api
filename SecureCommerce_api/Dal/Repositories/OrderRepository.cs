using Microsoft.EntityFrameworkCore;
using SecureCommerce_api.Dal.Entities;
using SecureCommerce_api.Dal.Repositories.Interfaces;

namespace SecureCommerce_api.Dal.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SecureCommerceContext _context;

        public OrderRepository(SecureCommerceContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message, Order? Order)> CheckoutAsync(
            Guid userId, 
            string fullName, 
            string address, 
            string city, 
            string zipCode, 
            string country, 
            string paymentMethod)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var cart = await _context.Carts
                .Include(existingCart => existingCart.CartItems)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(existingCart => existingCart.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return (false, "Cart is empty.", null);
            }

            foreach (var cartItem in cart.CartItems)
            {
                if (cartItem.Product == null)
                {
                    return (false, "One or more products no longer exist.", null);
                }

                var availableStock = cartItem.Product.Stock ?? 0;
                if (availableStock < cartItem.Quantity)
                {
                    return (false, $"Insufficient stock for product '{cartItem.Product.Name}'.", null);
                }
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Status = "Placed",
                ShippingFullName = fullName,
                ShippingAddress = address,
                ShippingCity = city,
                ShippingZipCode = zipCode,
                ShippingCountry = country,
                PaymentStatus = "Pending",
                PaymentMethod = paymentMethod,
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 0
            };

            var orderItems = cart.CartItems.Select(cartItem =>
            {
                var product = cartItem.Product!;
                var price = product.Price ?? 0;

                product.Stock = (product.Stock ?? 0) - cartItem.Quantity;

                return new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = price
                };
            }).ToList();

            order.TotalAmount = orderItems.Sum(item => (item.Price ?? 0) * (item.Quantity ?? 0));
            order.OrderItems = orderItems;

            _context.Orders.Add(order);
            _context.OrderItems.AddRange(orderItems);
            _context.CartItems.RemoveRange(cart.CartItems);
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var savedOrder = await _context.Orders
                .AsNoTracking()
                .Include(existingOrder => existingOrder.OrderItems)
                .ThenInclude(item => item.Product)
                .FirstAsync(existingOrder => existingOrder.Id == order.Id);

            return (true, "Order placed successfully.", savedOrder);
        }

        public async Task<IReadOnlyCollection<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(order => order.OrderItems)
                .ThenInclude(item => item.Product)
                .Where(order => order.UserId == userId)
                .OrderByDescending(order => order.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid userId, Guid orderId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(order => order.OrderItems)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(order => order.UserId == userId && order.Id == orderId);
        }

        public async Task<IReadOnlyCollection<Order>> GetOrdersByVendorIdAsync(Guid vendorId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(order => order.OrderItems)
                .ThenInclude(item => item.Product)
                .Where(order => order.OrderItems.Any(item => item.Product.VendorId == vendorId))
                .OrderByDescending(order => order.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
