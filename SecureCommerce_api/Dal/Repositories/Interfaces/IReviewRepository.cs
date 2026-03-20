using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecureCommerce_api.Dal.Entities;

namespace SecureCommerce_api.Dal.Repositories.Interfaces;

public interface IReviewRepository
{
    Task<IEnumerable<Review>> GetProductReviewsAsync(Guid productId);
    Task<Review?> GetUserReviewForProductAsync(Guid userId, Guid productId);
    Task<Review?> GetReviewByIdAsync(Guid id);
    Task<Review> CreateReviewAsync(Review review);
    Task<Review?> UpdateReviewAsync(Review review);
    Task<bool> DeleteReviewAsync(Guid id);
}
