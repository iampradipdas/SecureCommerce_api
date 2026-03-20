using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.Dal.Entities;
using SecureCommerce_api.Dal.Repositories.Interfaces;
using SecureCommerce_api.DTOs.Category;

namespace SecureCommerce_api.Bal;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _categoryRepository.GetCategoriesAsync();
        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        });
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(id);
        if (category == null) return null;

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto model)
    {
        var category = new Category
        {
            Name = model.Name,
            Description = model.Description
        };

        var created = await _categoryRepository.CreateCategoryAsync(category);
        return new CategoryDto
        {
            Id = created.Id,
            Name = created.Name,
            Description = created.Description
        };
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(Guid id, UpdateCategoryDto model)
    {
        var category = new Category
        {
            Id = id,
            Name = model.Name,
            Description = model.Description
        };

        var updated = await _categoryRepository.UpdateCategoryAsync(category);
        if (updated == null) return null;

        return new CategoryDto
        {
            Id = updated.Id,
            Name = updated.Name,
            Description = updated.Description
        };
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        return await _categoryRepository.DeleteCategoryAsync(id);
    }
}
