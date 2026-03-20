using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.Dal.Entities;
using SecureCommerce_api.Dal.Repositories.Interfaces;
using SecureCommerce_api.DTOs.Review;

namespace SecureCommerce_api.Bal;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(Guid productId)
    {
        var reviews = await _reviewRepository.GetProductReviewsAsync(productId);
        return reviews.Select(r => new ReviewDto
        {
            Id = r.Id,
            ProductId = r.ProductId,
            UserId = r.UserId,
            UserName = r.User?.Name ?? "Anonymous",
            Rating = r.Rating,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt ?? DateTime.UtcNow
        });
    }

    public async Task<ReviewDto> CreateReviewAsync(Guid userId, CreateReviewDto model)
    {
        var existing = await _reviewRepository.GetUserReviewForProductAsync(userId, model.ProductId);
        if (existing != null)
        {
            throw new Exception("You have already reviewed this product.");
        }

        var review = new Review
        {
            UserId = userId,
            ProductId = model.ProductId,
            Rating = model.Rating,
            Comment = model.Comment
        };

        var created = await _reviewRepository.CreateReviewAsync(review);
        return new ReviewDto
        {
            Id = created.Id,
            ProductId = created.ProductId,
            UserId = created.UserId,
            Rating = created.Rating,
            Comment = created.Comment,
            CreatedAt = created.CreatedAt ?? DateTime.UtcNow
        };
    }

    public async Task<ReviewDto?> UpdateReviewAsync(Guid userId, Guid id, UpdateReviewDto model)
    {
        var review = await _reviewRepository.GetReviewByIdAsync(id);
        if (review == null || review.UserId != userId) return null;

        review.Rating = model.Rating;
        review.Comment = model.Comment;

        var updated = await _reviewRepository.UpdateReviewAsync(review);
        if (updated == null) return null;

        return new ReviewDto
        {
            Id = updated.Id,
            ProductId = updated.ProductId,
            UserId = updated.UserId,
            Rating = updated.Rating,
            Comment = updated.Comment,
            CreatedAt = updated.CreatedAt ?? DateTime.UtcNow
        };
    }

    public async Task<bool> DeleteReviewAsync(Guid userId, Guid id)
    {
        var review = await _reviewRepository.GetReviewByIdAsync(id);
        if (review == null || review.UserId != userId) return false;

        return await _reviewRepository.DeleteReviewAsync(id);
    }
}
