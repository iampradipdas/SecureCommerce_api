using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.DTOs.Review;
using System.IdentityModel.Tokens.Jwt;

namespace SecureCommerce_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet("product/{productId:guid}")]
    public async Task<IActionResult> GetProductReviews(Guid productId)
    {
        var reviews = await _reviewService.GetProductReviewsAsync(productId);
        return Ok(reviews);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto model)
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

        try
        {
            var review = await _reviewService.CreateReviewAsync(userId.Value, model);
            return Ok(review);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateReview(Guid id, [FromBody] UpdateReviewDto model)
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

        var review = await _reviewService.UpdateReviewAsync(userId.Value, id, model);
        if (review == null)
        {
            return NotFound(new { Message = "Review not found or not authorized." });
        }

        return Ok(review);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteReview(Guid id)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "Invalid user token." });
        }

        var deleted = await _reviewService.DeleteReviewAsync(userId.Value, id);
        if (!deleted)
        {
            return NotFound(new { Message = "Review not found or not authorized." });
        }

        return NoContent();
    }

    private Guid? GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.NameId);

        return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : null;
    }
}
