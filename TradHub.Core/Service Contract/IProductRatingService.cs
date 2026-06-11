using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;

namespace TradHub.Core.Service_Contract
{
    public interface IProductRatingService
    {
        Task<RatingResponseDto> AddOrUpdateAsync(int productId, string userId, RatingRequestDto dto);
        Task<IReadOnlyList<RatingResponseDto>> GetProductRatingsAsync(int productId);
        Task<RatingSummaryDto> GetProductRatingSummaryAsync(int productId);
    }
}
