using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.DTOs.Category;

namespace SecureCommerce_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService.GetCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound(new { Message = "Category not found." });
        }

        return Ok(category);
    }

    [Authorize] // Should ideally be Admin only, but for now we'll allow authorized users
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var category = await _categoryService.CreateCategoryAsync(model);
        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var category = await _categoryService.UpdateCategoryAsync(id, model);
        if (category == null)
        {
            return NotFound(new { Message = "Category not found." });
        }

        return Ok(category);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var deleted = await _categoryService.DeleteCategoryAsync(id);
        if (!deleted)
        {
            return NotFound(new { Message = "Category not found." });
        }

        return NoContent();
    }
}
