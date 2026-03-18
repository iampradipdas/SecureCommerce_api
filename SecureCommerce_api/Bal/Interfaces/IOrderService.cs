using SecureCommerce_api.DTOs.Order;

namespace SecureCommerce_api.Bal.Interfaces
{
    public interface IOrderService
    {
        Task<(bool Success, string Message, OrderDto? Order)> CheckoutAsync(Guid userId);
        Task<IReadOnlyCollection<OrderDto>> GetOrdersAsync(Guid userId);
        Task<OrderDto?> GetOrderByIdAsync(Guid userId, Guid orderId);
    }
}
