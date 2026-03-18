using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.Dal.Entities;
using SecureCommerce_api.Dal.Repositories.Interfaces;
using SecureCommerce_api.DTOs.Order;

namespace SecureCommerce_api.Bal
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<(bool Success, string Message, OrderDto? Order)> CheckoutAsync(Guid userId)
        {
            var result = await _orderRepository.CheckoutAsync(userId);
            return result.Order == null
                ? (result.Success, result.Message, null)
                : (result.Success, result.Message, MapOrder(result.Order));
        }

        public async Task<IReadOnlyCollection<OrderDto>> GetOrdersAsync(Guid userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(MapOrder).ToList();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(Guid userId, Guid orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(userId, orderId);
            return order == null ? null : MapOrder(order);
        }

        private static OrderDto MapOrder(Order order)
        {
            var items = order.OrderItems.Select(item =>
            {
                var price = item.Price ?? 0;

                return new OrderItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId ?? Guid.Empty,
                    ProductName = item.Product?.Name ?? string.Empty,
                    Quantity = item.Quantity ?? 0,
                    Price = price,
                    LineTotal = price * (item.Quantity ?? 0)
                };
            }).ToList();

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount ?? 0,
                Status = order.Status ?? string.Empty,
                CreatedAt = order.CreatedAt,
                Items = items
            };
        }
    }
}
