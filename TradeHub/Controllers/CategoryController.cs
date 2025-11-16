using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
            if(categories is null || !categories.Any())
            {
                return NotFound(new ApiResponse(404,"No Categories Found."));
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
            catch(ArgumentException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            catch(Exception)
            {
                return StatusCode(500, new ApiResponse(500, "An error occurred while fetching the category."));
            }
        }
        [HttpGet("active-categories")]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetActiveCategories()
        {
            try
            {
                var result = await _categoryService.GetActiveAsync();
                return Ok(result);
            }
            catch (SqlException)
            {
                return StatusCode(500, new { message = "Database error occurred." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Something went wrong." });
            }
        }
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> AddCategory([FromBody] CategoryDto categoryDto)
        {
            try
            {
                var result = await _categoryService.AddAsync(categoryDto);
                if(result is null)
                {
                    return BadRequest(new ApiResponse(400, "Unable to add category."));
                }
                return Ok(result);
            }
            catch(DuplicateNameException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            catch(Exception)
            {
                return StatusCode(500, new ApiResponse(500, "An error occurred while adding the category."));
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            try
            {
                var result = await _categoryService.UpdateAsync(id, categoryDto);
                if (result is null)
                {
                    return NotFound(new ApiResponse(404, "Category Not Found."));
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            catch (DuplicateNameException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse(500, "An error occurred while updating the category."));
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoryDto>> DeleteCategory(int id)
        {
            try
            {
                var result = await _categoryService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new ApiResponse(404, "Category Not Found."));
                }
                return Ok(new ApiResponse(200, "Category Deleted Successfully."));
            }catch(ArgumentException ex)
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
