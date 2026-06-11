using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Repository;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service
{
    public class CompanyRatingsService : ICompanyRatingService
    {
        private readonly AppDbContext _context;

        public CompanyRatingsService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<RatingResponseDto> AddOrUpdateAsync(string companyId, string userId, RatingRequestDto dto)
        {
            if(dto.RatingValue < 1 || dto.RatingValue > 5)
            {
                throw new ArgumentException("Rating value must be between 1 and 5.");
            }
            var companyExists = _context.Companies.Any(c => c.CompanyId.ToString() == companyId);

            if(!companyExists)
            {
                throw new ArgumentException("Company not found.");
            }
            var rating = _context.CompanyRatings.FirstOrDefault(r => r.CompanyId.ToString() == companyId && r.UserId == userId);

            if (rating is null)
            {
                rating = new CompanyRatings
                {
                    CompanyId = Guid.Parse(companyId),
                    UserId = userId,
                    RatingValue = dto.RatingValue,
                    Comment = dto.Comment,
                    CreatedAt = DateTime.UtcNow
                };
                await _context.CompanyRatings.AddAsync(rating);
            }
            else
            {
                rating.RatingValue = dto.RatingValue;
                rating.Comment = dto.Comment;
                rating.UpdatedAt = DateTime.UtcNow;
                _context.CompanyRatings.Update(rating);
            }
            await _context.SaveChangesAsync();
            return new RatingResponseDto
            {
                Id = rating.RaitingId,
                RatingValue = rating.RatingValue,
                Comment = rating.Comment,
                UserId = rating.UserId,
                UserFullname = (await _context.Users.FindAsync(rating.UserId))?.FullName ?? "Unknown",
                CreatedAt = rating.CreatedAt
            };
        }

        public async Task<IReadOnlyList<RatingResponseDto>> GetCompanyRatingsAsync(string companyId)
        {
            return await _context.CompanyRatings
                .Where(r => r.CompanyId.ToString() == companyId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new RatingResponseDto
                {
                    Id = r.RaitingId,
                    RatingValue = r.RatingValue,
                    Comment = r.Comment,
                    UserId = r.UserId,
                    UserFullname = r.User.FullName,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<RatingSummaryDto> GetCompanyRatingSummaryAsync(string companyId)
        {
            var ratings = _context.CompanyRatings
            .Where(r => r.CompanyId.ToString() == companyId);

            var total = await ratings.CountAsync();

            var average = total == 0
                ? 0
                : await ratings.AverageAsync(r => r.RatingValue);

            return new RatingSummaryDto
            {
                AverageRating = Math.Round(average, 1),
                TotalRatings = total
            };
        }
    }
}
