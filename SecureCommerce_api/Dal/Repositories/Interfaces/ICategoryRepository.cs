using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecureCommerce_api.Dal.Entities;

namespace SecureCommerce_api.Dal.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(Guid id);
    Task<Category> CreateCategoryAsync(Category category);
    Task<Category?> UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(Guid id);
}
