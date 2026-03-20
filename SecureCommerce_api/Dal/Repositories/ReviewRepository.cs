using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecureCommerce_api.Dal.Entities;
using SecureCommerce_api.Dal.Repositories.Interfaces;

namespace SecureCommerce_api.Dal.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly SecureCommerceContext _context;

    public ReviewRepository(SecureCommerceContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Review>> GetProductReviewsAsync(Guid productId)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Where(r => r.ProductId == productId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Review?> GetUserReviewForProductAsync(Guid userId, Guid productId)
    {
        return await _context.Reviews
            .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);
    }

    public async Task<Review?> GetReviewByIdAsync(Guid id)
    {
        return await _context.Reviews.FindAsync(id);
    }

    public async Task<Review> CreateReviewAsync(Review review)
    {
        review.Id = Guid.NewGuid();
        review.CreatedAt = DateTime.UtcNow;
        review.UpdatedAt = DateTime.UtcNow;
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        return review;
    }

    public async Task<Review?> UpdateReviewAsync(Review review)
    {
        var existing = await _context.Reviews.FindAsync(review.Id);
        if (existing == null) return null;

        existing.Rating = review.Rating;
        existing.Comment = review.Comment;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteReviewAsync(Guid id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null) return false;

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
        return true;
    }
}
