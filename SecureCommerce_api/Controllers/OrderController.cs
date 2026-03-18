using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureCommerce_api.Bal.Interfaces;

namespace SecureCommerce_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { Message = "Invalid user token." });
            }

            var result = await _orderService.CheckoutAsync(userId.Value);
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(result.Order);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { Message = "Invalid user token." });
            }

            var orders = await _orderService.GetOrdersAsync(userId.Value);
            return Ok(orders);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { Message = "Invalid user token." });
            }

            var order = await _orderService.GetOrderByIdAsync(userId.Value, id);
            if (order == null)
            {
                return NotFound(new { Message = "Order not found." });
            }

            return Ok(order);
        }

        private Guid? GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.NameId);

            return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : null;
        }
    }
}
