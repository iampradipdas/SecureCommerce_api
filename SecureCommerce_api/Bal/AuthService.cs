using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.Dal.Entities;
using SecureCommerce_api.Dal.Repositories.Interfaces;
using SecureCommerce_api.DTOs.Auth;
using BCrypt.Net;

namespace SecureCommerce_api.Bal
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto model)
        {
            if (await _authRepository.UserExistsAsync(model.Email))
            {
                return new AuthResponseDto { Success = false, Message = "User already exists with this email." };
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                IsActive = true
            };

            await _authRepository.CreateUserAsync(user);

            return new AuthResponseDto { Success = true, Message = "User registered successfully." };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto model)
        {
            var user = await _authRepository.GetUserByEmailAsync(model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return new AuthResponseDto { Success = false, Message = "Invalid email or password." };
            }

            if (user.IsActive == false)
            {
                return new AuthResponseDto { Success = false, Message = "User account is inactive." };
            }

            var token = GenerateJwtToken(user);
            var refreshToken = await CreateRefreshTokenAsync(user.Id);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Login successful.",
                Token = token,
                RefreshToken = refreshToken.Token,
                User = new UserInfo
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                }
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto model)
        {
            var existingRefreshToken = await _authRepository.GetRefreshTokenAsync(model.RefreshToken);
            if (existingRefreshToken?.User == null)
            {
                return new AuthResponseDto { Success = false, Message = "Invalid refresh token." };
            }

            if (existingRefreshToken.IsRevoked == true || existingRefreshToken.ExpiryDate <= DateTime.UtcNow)
            {
                return new AuthResponseDto { Success = false, Message = "Refresh token has expired or was revoked." };
            }

            if (existingRefreshToken.User.IsActive == false)
            {
                return new AuthResponseDto { Success = false, Message = "User account is inactive." };
            }

            existingRefreshToken.IsRevoked = true;
            await _authRepository.UpdateRefreshTokenAsync(existingRefreshToken);

            var accessToken = GenerateJwtToken(existingRefreshToken.User);
            var newRefreshToken = await CreateRefreshTokenAsync(existingRefreshToken.User.Id);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Token refreshed successfully.",
                Token = accessToken,
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task<AuthResponseDto> RevokeRefreshTokenAsync(RefreshTokenRequestDto model)
        {
            var existingRefreshToken = await _authRepository.GetRefreshTokenAsync(model.RefreshToken);
            if (existingRefreshToken == null)
            {
                return new AuthResponseDto { Success = false, Message = "Refresh token not found." };
            }

            if (existingRefreshToken.IsRevoked == true)
            {
                return new AuthResponseDto { Success = true, Message = "Refresh token already revoked." };
            }

            existingRefreshToken.IsRevoked = true;
            await _authRepository.UpdateRefreshTokenAsync(existingRefreshToken);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Refresh token revoked successfully."
            };
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private async Task<RefreshToken> CreateRefreshTokenAsync(Guid userId)
        {
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = GenerateSecureRefreshToken(),
                ExpiryDate = DateTime.UtcNow.AddDays(GetRefreshTokenExpiryDays()),
                IsRevoked = false
            };

            return await _authRepository.CreateRefreshTokenAsync(refreshToken);
        }

        private string GenerateSecureRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }

        private int GetRefreshTokenExpiryDays()
        {
            return int.TryParse(_configuration["Jwt:RefreshTokenExpiryDays"], out var expiryDays)
                ? expiryDays
                : 7;
        }
    }
}
