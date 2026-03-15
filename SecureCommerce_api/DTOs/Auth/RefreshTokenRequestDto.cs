using System.ComponentModel.DataAnnotations;

namespace SecureCommerce_api.DTOs.Auth
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
