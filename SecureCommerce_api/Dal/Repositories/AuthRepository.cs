using Microsoft.EntityFrameworkCore;
using SecureCommerce_api.Dal.Entities;
using SecureCommerce_api.Dal.Repositories.Interfaces;

namespace SecureCommerce_api.Dal.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly SecureCommerceContext _context;

        public AuthRepository(SecureCommerceContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                    .ThenInclude(u => u!.Role)
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task AssignRoleAsync(Guid userId, int roleId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.RoleId = roleId;
                await _context.SaveChangesAsync();
            }
        }
    }
}
