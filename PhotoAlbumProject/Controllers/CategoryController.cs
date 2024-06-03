using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Application.DTOs;
using PhotoAlbum.Application.Interfaces;
using PhotoAlbumProject.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoAlbumProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetCategories")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(new ApiResponse<IEnumerable<CategoryDto>>(true, "Categories retrieved successfully", categories));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<CategoryDto>>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpGet("GetCategory/{id}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategory(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return NotFound(new ApiResponse<CategoryDto>(false, "Category not found"));
                }
                return Ok(new ApiResponse<CategoryDto>(true, "Category retrieved successfully", category));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CategoryDto>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpPost("CreateCategory")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> CreateCategory(CategoryDto categoryDto)
        {
            try
            {
                var createdCategory = await _categoryService.AddCategoryAsync(categoryDto);
                return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, new ApiResponse<CategoryDto>(true, "Category created successfully", createdCategory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CategoryDto>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDto categoryDto)
        {
            try
            {
                if (id != categoryDto.Id)
                {
                    return BadRequest(new ApiResponse<CategoryDto>(false, "ID mismatch"));
                }

                await _categoryService.UpdateCategoryAsync(categoryDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CategoryDto>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CategoryDto>(false, $"Internal server error: {ex.Message}"));
            }
        }
    }
}
