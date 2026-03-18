using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.DTOs.Cart;

namespace SecureCommerce_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { Message = "Invalid user token." });
            }

            var cart = await _cartService.GetCartAsync(userId.Value);
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] AddCartItemDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { Message = "Invalid user token." });
            }

            var result = await _cartService.AddItemAsync(userId.Value, model);
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(result.Cart);
        }

        [HttpPut("items/{productId:guid}")]
        public async Task<IActionResult> UpdateItem(Guid productId, [FromBody] UpdateCartItemDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { Message = "Invalid user token." });
            }

            var result = await _cartService.UpdateItemAsync(userId.Value, productId, model);
            if (!result.Success)
            {
                return result.Message == "Requested quantity exceeds available stock."
                    ? BadRequest(new { result.Message })
                    : NotFound(new { result.Message });
            }

            return Ok(result.Cart);
        }

        [HttpDelete("items/{productId:guid}")]
        public async Task<IActionResult> RemoveItem(Guid productId)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { Message = "Invalid user token." });
            }

            var result = await _cartService.RemoveItemAsync(userId.Value, productId);
            if (!result.Success)
            {
                return NotFound(new { result.Message });
            }

            return Ok(result.Cart);
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { Message = "Invalid user token." });
            }

            var result = await _cartService.ClearCartAsync(userId.Value);
            return Ok(new { result.Message });
        }

        private Guid? GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.NameId);

            return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : null;
        }
    }
}
