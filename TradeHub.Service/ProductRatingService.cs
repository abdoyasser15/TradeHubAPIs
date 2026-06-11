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

    public class ProductRatingService : IProductRatingService
    {
        private readonly AppDbContext _context;

        public ProductRatingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RatingResponseDto> AddOrUpdateAsync(
            int productId,
            string userId,
            RatingRequestDto dto)
        {
            if (dto.RatingValue < 1 || dto.RatingValue > 5)
                throw new ArgumentException("Rating value must be between 1 and 5");

            var productExists = await _context.Products.AnyAsync(p => p.Id == productId);

            if (!productExists)
                throw new KeyNotFoundException("Product not found");

            var rating = await _context.productRatings
                .FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);

            if (rating is null)
            {
                rating = new ProductRating
                {
                    ProductId = productId,
                    UserId = userId,
                    RaitngValue = dto.RatingValue,
                    Comment = dto.Comment,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.productRatings.AddAsync(rating);
            }
            else
            {
                rating.RaitngValue = dto.RatingValue;
                rating.Comment = dto.Comment;
            }

            await _context.SaveChangesAsync();

            return new RatingResponseDto
            {
                Id = rating.RaitingId,
                RatingValue = rating.RaitngValue,
                Comment = rating.Comment,
                UserId = rating.UserId,
                UserFullname = (await _context.Users.FindAsync(rating.UserId))?.FullName ?? "Unknown",
                CreatedAt = rating.CreatedAt
            };
        }

        public async Task<IReadOnlyList<RatingResponseDto>> GetProductRatingsAsync(int productId)
        {
            return await _context.productRatings
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new RatingResponseDto
                {
                    Id = r.RaitingId,
                    RatingValue = r.RaitngValue,
                    Comment = r.Comment,
                    UserId = r.UserId,
                    UserFullname = r.User.FullName,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<RatingSummaryDto> GetProductRatingSummaryAsync(int productId)
        {
            var ratings = _context.productRatings
                .Where(r => r.ProductId == productId);

            var total = await ratings.CountAsync();

            var average = total == 0
                ? 0
                : await ratings.AverageAsync(r => r.RaitngValue);

            return new RatingSummaryDto
            {
                AverageRating = Math.Round(average, 1),
                TotalRatings = total
            };
        }
    }
}
