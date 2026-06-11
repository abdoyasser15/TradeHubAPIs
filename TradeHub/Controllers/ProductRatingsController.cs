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
    [Route("api/products/{productId:int}/ratings")]
    public class ProductRatingsController : ControllerBase
    {
        private readonly IProductRatingService _productRatingService;

        public ProductRatingsController(IProductRatingService productRatingService)
        {
            _productRatingService = productRatingService;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RatingResponseDto>> AddOrUpdateRating(
        int productId,
        RatingRequestDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401));

            var result = await _productRatingService.AddOrUpdateAsync(productId, userId, dto);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RatingResponseDto>>> GetRatings(int productId)
        {
            var ratings = await _productRatingService.GetProductRatingsAsync(productId);

            return Ok(ratings);
        }

        [HttpGet("summary")]
        public async Task<ActionResult<RatingSummaryDto>> GetSummary(int productId)
        {
            var summary = await _productRatingService.GetProductRatingSummaryAsync(productId);

            return Ok(summary);
        }
    }
}
