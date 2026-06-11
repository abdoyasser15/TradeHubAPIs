using Microsoft.AspNetCore.Mvc;
using System.Data;
using TradeHub.Errors;
using TradHub.Core.Dtos;
using TradHub.Core.Service_Contract;

namespace TradeHub.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryService.GetAllAsync();

            if (categories is null || !categories.Any())
            {
                return NotFound(new ApiResponse(404, "No Categories Found."));
            }

            return Ok(categories);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if (category is null)
                {
                    return NotFound(new ApiResponse(404, "Category Not Found."));
                }
                return Ok(category);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse(500, "An error occurred while fetching the category."));
            }
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromForm] CreateCategoryDto dto)
        {
            var result = await _categoryService.AddAsync(dto);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromForm] UpdateCategoryDto dto)
        {
            var result = await _categoryService.UpdateAsync(id, dto);

            if (result is null)
                return NotFound(new { message = "Category not found" });

            return Ok(result);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                var deleted = await _categoryService.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound(new ApiResponse(404, "Category Not Found."));
                }
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse(500, "An error occurred while deleting the category."));
            }
        }
    }
}
