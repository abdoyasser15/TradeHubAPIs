using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TradeHub.Errors;
using TradHub.Core.Dtos;
using TradHub.Core.Service_Contract;

namespace TradeHub.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId:int}/ratings")]
    public class CompanyRatingsController : ControllerBase
    {
        private readonly ICompanyRatingService _companyRatingService;

        public CompanyRatingsController(ICompanyRatingService companyRatingService)
        {
            _companyRatingService = companyRatingService;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RatingResponseDto>> AddOrUpdateRating(
        string companyId,
        RatingRequestDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401));

            var result = await _companyRatingService.AddOrUpdateAsync(companyId, userId, dto);

            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RatingResponseDto>>> GetRatings(string companyId)
        {
            var ratings = await _companyRatingService.GetCompanyRatingsAsync(companyId);

            return Ok(ratings);
        }
        [HttpGet("summary")]
        public async Task<ActionResult<RatingSummaryDto>> GetSummary(string companyId)
        {
            var summary = await _companyRatingService.GetCompanyRatingSummaryAsync(companyId);

            return Ok(summary);
        }
    }
}
