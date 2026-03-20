using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecureCommerce_api.DTOs.Review;

namespace SecureCommerce_api.Bal.Interfaces;

public interface IReviewService
{
    Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(Guid productId);
    Task<ReviewDto> CreateReviewAsync(Guid userId, CreateReviewDto model);
    Task<ReviewDto?> UpdateReviewAsync(Guid userId, Guid id, UpdateReviewDto model);
    Task<bool> DeleteReviewAsync(Guid userId, Guid id);
}
