using SecureCommerce_api.DTOs.Cart;

namespace SecureCommerce_api.Bal.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(Guid userId);
        Task<(bool Success, string Message, CartDto? Cart)> AddItemAsync(Guid userId, AddCartItemDto model);
        Task<(bool Success, string Message, CartDto? Cart)> UpdateItemAsync(Guid userId, Guid productId, UpdateCartItemDto model);
        Task<(bool Success, string Message, CartDto? Cart)> RemoveItemAsync(Guid userId, Guid productId);
        Task<(bool Success, string Message)> ClearCartAsync(Guid userId);
    }
}
