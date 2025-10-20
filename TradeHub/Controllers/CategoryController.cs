using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var category = await _categoryService.GetByIdAsync(id);
            if(category is null)
            {
                return NotFound(new ApiResponse(404, "Category Not Found."));
            }
            return Ok(category);
        }
        [HttpGet("active-categories")]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetActiveCategories()
        {
            var categories = await _categoryService.GetActiveAsync();
            if(categories is null || !categories.Any())
            {
                return NotFound(new ApiResponse(404, "No Active Categories Found."));
            }
            return Ok(categories);
        }
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> AddCategory([FromBody] CategoryDto categoryDto)
        {
            var createdCategory = await _categoryService.AddAsync(categoryDto);
            if(createdCategory is null)
            {
                return BadRequest(new ApiResponse(400, "Problem Creating Category."));
            }
            return Ok(createdCategory);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            var updatedCategory = await _categoryService.UpdateAsync(id, categoryDto);
            if(updatedCategory is null)
            {
                return NotFound(new ApiResponse(404, "Category Not Found."));
            }
            return Ok(updatedCategory);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoryDto>> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if(!result)
            {
                return NotFound(new ApiResponse(404, "Category Not Found."));
            }
            return Ok(new ApiResponse(200, "Category Deleted Successfully."));
        }
    }
}
