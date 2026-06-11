using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;
using TradHub.Core.Specifications.OrderSpec;

namespace TradeHub.Service
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderFromBasketAsync(string buyerId, CreateOrderFromBasketRequest request, CancellationToken cancellationToken = default);
        Task<Pagination<OrderDto>> GetOrdersAsync(string buyerId,OrderSpecParams specParams, CancellationToken cancellationToken = default);
        Task<OrderDto?> GetOrderByIdAsync(int orderId, string buyerId, CancellationToken cancellationToken = default);
    }
}
