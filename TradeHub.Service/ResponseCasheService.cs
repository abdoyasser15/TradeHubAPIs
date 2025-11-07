using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service
{
    public class ResponseCasheService : IResponseCashService
    {
        private readonly StackExchange.Redis.IDatabase _database;

        public ResponseCasheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task CashResponseAsync(string CashKey, object Response, TimeSpan TimeToLive)
        {
            if (Response is null)
                return ;

            var serializeOtions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var serializedResponse = JsonSerializer.Serialize(Response);

            await _database.StringSetAsync(CashKey, serializedResponse, TimeToLive);
        }

        public async Task<string?> GetCashedResonseAsync(string CashKey)
        {
            var cachedResponse = await _database.StringGetAsync(CashKey);

            if (cachedResponse.IsNullOrEmpty)
                return null;
            return cachedResponse.ToString();
        }
    }
}
