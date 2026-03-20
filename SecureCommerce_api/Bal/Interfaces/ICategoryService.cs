using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecureCommerce_api.DTOs.Category;

namespace SecureCommerce_api.Bal.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto model);
    Task<CategoryDto?> UpdateCategoryAsync(Guid id, UpdateCategoryDto model);
    Task<bool> DeleteCategoryAsync(Guid id);
}
