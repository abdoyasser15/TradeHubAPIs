using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Repository;
using TradHub.Core.Dtos;
using TradHub.Core.Entity.Basket;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service
{
    public class BasketService : IBasketService
    {
        private readonly AppDbContext _context;

        public BasketService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<BasketDto> AddItemAsync(string buyerId, AddBasketItemDto request)
        {
            if (request.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            request.SelectedOptionValueIds ??= new List<int>();

            var product = await _context.Products
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == request.ProductId);

            if (product is null)
                throw new Exception("Product not found.");

            var selectedValues = await _context.ProductOptionValues
                .Include(v => v.ProductOptions)
                .Where(v => request.SelectedOptionValueIds.Contains(v.ProductOptionValueId))
                .ToListAsync();

            if (selectedValues.Count != request.SelectedOptionValueIds.Count)
                throw new Exception("One or more selected options are invalid.");

            var invalidOptions = selectedValues
                .Where(v => v.ProductOptions.ProductId != product.Id)
                .ToList();

            if (invalidOptions.Any())
                throw new Exception("One or more selected options do not belong to this product.");

            var optionsExtraPrice = selectedValues.Sum(v => v.ExtraPrice);

            var basket = await _context.Baskets
                .Include(x => x.Items)
                    .ThenInclude(i => i.Options)
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x =>
                    x.BuyerId == buyerId &&
                    x.CompanyId == product.CompanyId);

            if (basket is null)
            {
                basket = new Basket
                {
                    BuyerId = buyerId,
                    CompanyId = product.CompanyId,
                    Company = product.Company,
                    Items = new List<BasketItem>()
                };

                _context.Baskets.Add(basket);
            }

            var selectedIds = request.SelectedOptionValueIds
                .OrderBy(x => x)
                .ToList();

            var existingItem = basket.Items.FirstOrDefault(item =>
                item.ProductId == request.ProductId &&
                item.Options
                    .Select(o => o.ProductOptionValueId)
                    .OrderBy(id => id)
                    .SequenceEqual(selectedIds));

            if (existingItem is not null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                basket.Items.Add(new BasketItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name ?? string.Empty,
                    LogoUrl = product.ImageUrl ?? string.Empty,
                    Price = product.Price + optionsExtraPrice,
                    Quantity = request.Quantity,
                    Options = selectedValues.Select(v => new BasketItemOptions
                    {
                        ProductOptionValueId = v.ProductOptionValueId,
                        OptionName = v.ProductOptions.Name,
                        ValueName = v.Name,
                        ExtraPrice = v.ExtraPrice
                    }).ToList()
                });
            }

            await _context.SaveChangesAsync();

            var savedBasket = await _context.Baskets
                .AsNoTracking()
                .Include(x => x.Company)
                .Include(x => x.Items)
                    .ThenInclude(i => i.Options)
                .FirstOrDefaultAsync(x =>
                    x.BuyerId == buyerId &&
                    x.CompanyId == product.CompanyId);

            return MapToDto(savedBasket!);
        }
        public async Task<bool> ClearBasketAsync(int basketId)
        {
            var basket = await _context.Baskets
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == basketId);

            if (basket is null)
                return false;

            _context.BasketItems.RemoveRange(basket.Items);

            _context.Baskets.Remove(basket);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<BasketDto>> GetBasketAsync(string buyerId)
        {
            var baskets = await _context.Baskets
                .Include(x => x.Company)
                .Include(x => x.Items)
                    .ThenInclude(i => i.Options)
                .Where(x => x.BuyerId == buyerId)
                .ToListAsync();

            if (baskets is null || !baskets.Any())
            {
                return new List<BasketDto>();
            }

            return baskets.Select(MapToDto).ToList();
        }

        public async Task<BasketDto> RemoveItemAsync(int basketId, int productId)
        {
            var basket = await _context.Baskets
                .Include(x => x.Company)
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == basketId);

            if (basket is null)
                throw new Exception("Basket not found.");

            var item = basket.Items
                .FirstOrDefault(x => x.ProductId == productId);

            if (item is null)
                throw new Exception("Item not found in basket.");

            basket.Items.Remove(item);

            if (!basket.Items.Any())
            {
                _context.Baskets.Remove(basket);
            }

            await _context.SaveChangesAsync();

            return MapToDto(basket);
        }
        public async Task<BasketDto> UpdateQuantityAsync(
             int basketId,
             int productId,
             int quantity)
        {
            var basket = await _context.Baskets
                .Include(x => x.Company)
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == basketId);

            if (basket is null)
                throw new Exception("Basket not found.");

            var item = basket.Items
                .FirstOrDefault(x => x.ProductId == productId);

            if (item is null)
                throw new Exception("Item not found.");

            if (quantity <= 0)
            {
                basket.Items.Remove(item);

                if (!basket.Items.Any())
                {
                    _context.Baskets.Remove(basket);
                }
            }
            else
            {
                item.Quantity = quantity;
            }

            await _context.SaveChangesAsync();

            return MapToDto(basket);
        }
        private static BasketDto MapToDto(Basket basket)
        {
            return new BasketDto
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                CompanyId = basket.CompanyId,
                CompanyName = basket.Company?.BusinessName ?? string.Empty,
                LogoUrl = basket.Company?.LogoUrl ?? string.Empty,

                Items = basket.Items.Select(x => new BasketItemDto
                {
                    Id = x.Id,
                    ProductName = x.ProductName,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    PictureUrl = x.LogoUrl,

                    Options = x.Options.Select(o => new BasketItemOptionsDto
                    {
                        ProductOptionValueId = o.ProductOptionValueId,
                        OptionName = o.OptionName,
                        ValueName = o.ValueName,
                        ExtraPrice = o.ExtraPrice
                    }).ToList()
                }).ToList()
            };
        }
    }
}
