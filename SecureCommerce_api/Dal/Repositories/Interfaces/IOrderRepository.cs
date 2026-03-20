using SecureCommerce_api.Dal.Entities;

namespace SecureCommerce_api.Dal.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<(bool Success, string Message, Order? Order)> CheckoutAsync(Guid userId, string fullName, string address, string city, string zipCode, string country, string paymentMethod);
        Task<IReadOnlyCollection<Order>> GetOrdersByUserIdAsync(Guid userId);
        Task<Order?> GetOrderByIdAsync(Guid userId, Guid orderId);
    }
}
