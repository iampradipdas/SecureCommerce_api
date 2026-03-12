using SecureCommerce_api.DTOs.Auth;

namespace SecureCommerce_api.Bal.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto model);
        Task<AuthResponseDto> LoginAsync(LoginDto model);
    }
}
