using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;

namespace TradHub.Core.Service_Contract
{
    public interface ICompanyRatingService
    {
        Task<RatingResponseDto> AddOrUpdateAsync(string companyId, string userId, RatingRequestDto dto);
        Task<IReadOnlyList<RatingResponseDto>> GetCompanyRatingsAsync(string companyId);
        Task<RatingSummaryDto> GetCompanyRatingSummaryAsync(string companyId);
    }
}
