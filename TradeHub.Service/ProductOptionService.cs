using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Repository;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service
{
    public class ProductOptionService : IProductOptionsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public ProductOptionService(IUnitOfWork unitOfWork , AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }
        public async Task<ProductOptionDto?> AddOptionToProductAsync(int productId, CreateProductOptionDto dto)
        {
            var product = await _unitOfWork.Repository<Product>().GetById(productId);

            if (product is null) return null;

            var option = new ProductOptions
            {
                Name = dto.Name,
                IsRequired = dto.IsRequired,
                AllowMultiple = dto.AllowMultiple,
                ProductId = productId,
                ProductOptionValues = dto.Values.Select(v => new ProductOptionValue
                {
                    Name = v.Name,
                    ExtraPrice = v.ExtraPrice
                }).ToList()
            };

            await _unitOfWork.Repository<ProductOptions>().AddAsync(option);
            await _unitOfWork.CompleteAsync();

            return new ProductOptionDto
            {
                Id = option.ProductOptionId,
                Name = option.Name,
                IsRequired = option.IsRequired,
                AllowMultiple = option.AllowMultiple,
                Values = option.ProductOptionValues.Select(v => new ProductOptionValueDto
                {
                    Id = v.ProductOptionValueId,
                    Name = v.Name,
                    ExtraPrice = v.ExtraPrice
                }).ToList()
            };
        }
        public async Task<IReadOnlyList<ProductOptionDto>> GetOptionsByProductIdAsync(int productId)
        {
            var options = await _context.ProductOptions
                .Where(o => o.ProductId == productId)
                .Include(o => o.ProductOptionValues)
                .ToListAsync();

            return options
                .Where(o => o.ProductId == productId)
                .Select(o => new ProductOptionDto
                {
                    Id = o.ProductOptionId,
                    Name = o.Name,
                    IsRequired = o.IsRequired,
                    AllowMultiple = o.AllowMultiple,
                    Values = o.ProductOptionValues.Select(v => new ProductOptionValueDto
                    {
                        Id = v.ProductOptionValueId,
                        Name = v.Name,
                        ExtraPrice = v.ExtraPrice
                    }).ToList()
                }).ToList();    
        }
        public async Task<bool> DeleteOptionAsync(int optionId)
        {
            var option = await _unitOfWork.Repository<ProductOptions>().GetById(optionId);

            if(option is null) return false;

            _unitOfWork.Repository<ProductOptions>().DeleteAsync(option);
            await _unitOfWork.CompleteAsync();
            return true;
        } 

    }
}
