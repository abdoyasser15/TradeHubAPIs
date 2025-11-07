using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using TradHub.Core.Service_Contract;

namespace TradeHub.Helpers
{
    public class CashedAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ResponseCashingService = context.HttpContext.RequestServices.GetRequiredService<IResponseCashService>();
            var cacheKey = GenerateCacheKeyFromRequest(context);
            var cachedResponse = ResponseCashingService.GetCashedResonseAsync(cacheKey);
            if (cachedResponse != null && cachedResponse.Result != null)
            {
                var contentResult = new Microsoft.AspNetCore.Mvc.ContentResult
                {
                    Content = cachedResponse.Result,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return ;
            }
            var executedContext = await next.Invoke();
            if (executedContext.Result is Microsoft.AspNetCore.Mvc.ObjectResult objectResult)
            {
                await ResponseCashingService.CashResponseAsync(cacheKey, objectResult.Value,TimeSpan.FromMinutes(5));
            }
        }

        private string GenerateCacheKeyFromRequest(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
