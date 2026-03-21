using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.DTOs.Order;

namespace SecureCommerce_api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        public async Task<IActionResult> Checkout([FromBody] CheckoutDto model)
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

            var result = await _orderService.CheckoutAsync(userId.Value, model);
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

        [HttpGet("vendor")]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> GetVendorOrders()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { Message = "Invalid user token." });
            }

            var orders = await _orderService.GetVendorOrdersAsync(userId.Value);
            return Ok(orders);
        }

        [HttpPut("{id:guid}/status")]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] string status)
        {
            var result = await _orderService.UpdateOrderStatusAsync(id, status);
            if (!result)
            {
                return NotFound(new { Message = "Order not found." });
            }

            return Ok(new { Message = "Order status updated successfully." });
        }

        private Guid? GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.NameId);

            return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : null;
        }
    }
}
