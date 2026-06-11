using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;
using TradHub.Core.Entity.Orders;

namespace TradeHub.Service
{
    public interface IPaymentGatewayService
    {
        Task<CreatePaymentSessionResponse> CreatePaymobSessionAsync(Order order, CancellationToken cancellationToken = default);
        Task HandlePaymobWebhookAsync(
            string rawBody,
            IHeaderDictionary headers,
            IQueryCollection query,
            CancellationToken cancellationToken = default);
    }
}
