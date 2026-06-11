using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;

namespace TradHub.Core.Service_Contract
{
    public interface IBasketService
    {
        Task<List<BasketDto>> GetBasketAsync(string buyerId);
        Task<BasketDto> AddItemAsync(string buyerId, AddBasketItemDto request);
        Task<BasketDto> RemoveItemAsync(int basketId, int productId);
        Task<BasketDto> UpdateQuantityAsync(int basketId, int productId, int quantity);
        Task<bool> ClearBasketAsync(int basketId);
    }
}
