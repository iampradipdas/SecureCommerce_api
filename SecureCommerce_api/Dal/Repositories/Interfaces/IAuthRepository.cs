using SecureCommerce_api.Dal.Entities;

namespace SecureCommerce_api.Dal.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
    }
}
