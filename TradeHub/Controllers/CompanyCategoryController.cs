using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradeHub.Errors;
using TradHub.Core.Dtos;
using TradHub.Core.Service_Contract;

namespace TradeHub.Controllers
{
    public class CompanyCategoryController : BaseApiController
    {
        private readonly ICompanyCategoryService _companyCategoryService;

        public CompanyCategoryController(ICompanyCategoryService companyCategoryService)
        {
            _companyCategoryService = companyCategoryService;
        }
        [HttpPost]
        public async Task<ActionResult> AddCompanyCategory([FromBody]CompanyCategoryCreateDto dto)
        {
            var result = await _companyCategoryService.AddAsync(dto);
            if (!result)
                return BadRequest(new ApiResponse(400,"CompanyCategory already exists."));
            return Ok("CompanyCategory added Successfully");
        }
        [HttpGet("{companyId}")]
        public async Task<ActionResult<IReadOnlyList<CompanyCategoryDto>>> GetCompanyCategoriesByCompanyId(Guid companyId)
        {
            var companyCategories = await _companyCategoryService.GetByCompanyIdAsync(companyId);
            return Ok(companyCategories);
        }
        [HttpDelete("{companyId}/{categoryId}")]
        public async Task<ActionResult> RemoveCompanyCategory(Guid companyId, int categoryId)
        {
            var result = await _companyCategoryService.RemoveAsync(companyId, categoryId);
            if (!result)
                return NotFound("CompanyCategory not found.");
            return Ok(new ApiResponse(200,"CompanyCategory removed successfully."));
        }
    }
}
