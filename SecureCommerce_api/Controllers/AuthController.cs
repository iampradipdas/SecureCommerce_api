using Microsoft.AspNetCore.Mvc;
using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.DTOs.Auth;

namespace SecureCommerce_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.RegisterAsync(model);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.LoginAsync(model);
            if (!response.Success)
                return Unauthorized(response);

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.RefreshTokenAsync(model);
            if (!response.Success)
                return Unauthorized(response);

            return Ok(response);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RefreshTokenRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.RevokeRefreshTokenAsync(model);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
