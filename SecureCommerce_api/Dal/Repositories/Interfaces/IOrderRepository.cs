using SecureCommerce_api.Dal.Entities;

namespace SecureCommerce_api.Dal.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<(bool Success, string Message, Order? Order)> CheckoutAsync(Guid userId);
        Task<IReadOnlyCollection<Order>> GetOrdersByUserIdAsync(Guid userId);
        Task<Order?> GetOrderByIdAsync(Guid userId, Guid orderId);
    }
}
