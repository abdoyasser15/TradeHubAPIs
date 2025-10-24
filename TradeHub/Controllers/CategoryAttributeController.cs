using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradeHub.Errors;
using TradHub.Core.Dtos;
using TradHub.Core.Service_Contract;

namespace TradeHub.Controllers
{
    public class CategoryAttributeController : BaseApiController
    {
        private readonly ICategoryAttributeService _categoryAttributeService;

        public CategoryAttributeController(ICategoryAttributeService categoryAttributeService)
        {
            _categoryAttributeService = categoryAttributeService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoryAttributeDto>>> GetAllCategoryAttributes()
        {
            var categoryAttributes = await _categoryAttributeService.GetAllCategoryAttribute();
            if(categoryAttributes == null || !categoryAttributes.Any())
            {
                return NotFound(new ApiResponse(404, "No category attributes found."));
            }
            return Ok(categoryAttributes);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryAttributeDto>> GetCategoryAttributeById(int id)
        {
            var categoryAttribute = await _categoryAttributeService.GetCategoryAttributeById(id);
            if(categoryAttribute == null)
            {
                return NotFound(new ApiResponse(404, "Category attribute not found."));
            }
            return Ok(categoryAttribute);
        }
        [HttpPost]
        public async Task<ActionResult> AddCategoryAttribute(CategoryAttributeCreateDto dto)
        {
            var result = await _categoryAttributeService.AddCategoryAttribute(dto);
            if(!result)
            {
                return BadRequest(new ApiResponse(400, "Failed to add category attribute."));
            }
            return Ok(new ApiResponse(200, "Category attribute added successfully."));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategoryAttribute(int id, CategoryAttributeUpdateDto dto)
        {
            var result = await _categoryAttributeService.UpdateCategoryAttribute(id, dto);
            if(!result)
            {
                return BadRequest(new ApiResponse(400, "Failed to update category attribute."));
            }
            return Ok(new ApiResponse(200, "Category attribute updated successfully."));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoryAttribute(int id)
        {
            var result = await _categoryAttributeService.DeleteCategoryAttribute(id);
            if(!result)
            {
                return BadRequest(new ApiResponse(400, "Failed to delete category attribute."));
            }
            return Ok(new ApiResponse(200, "Category attribute deleted successfully."));
        }
    }
}
