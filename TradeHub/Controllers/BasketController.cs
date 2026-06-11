using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TradeHub.Errors;
using TradHub.Core.Dtos;
using TradHub.Core.Service_Contract;

namespace TradeHub.Controllers
{
    [Authorize]
    public class BasketController : BaseApiController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var basket = await _basketService.GetBasketAsync(buyerId!);

            return Ok(basket);
        }
        [HttpPost("items")]
        public async Task<IActionResult> AddItem(AddBasketItemDto request)
        {
            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var basket = await _basketService.AddItemAsync(buyerId!, request);

            return Ok(basket);
        }
        [HttpDelete("{basketId}/items/{productId}")]
        public async Task<IActionResult> RemoveItem(int basketId, int productId)
        {
            var basket = await _basketService.RemoveItemAsync(basketId, productId);

            return Ok(new ApiResponse(200,"Deleted Successfully"));
        }
        [HttpPut("{basketId}/items/{productId}")]
        public async Task<IActionResult> UpdateQuantity(
            int basketId,
            int productId,
            [FromQuery] int quantity)
        {
            var basket = await _basketService.UpdateQuantityAsync(
                basketId,
                productId,
                quantity);

            return Ok(basket);
        }
        [HttpDelete("{basketId}")]
        public async Task<IActionResult> ClearBasket(int basketId)
        {
            var result = await _basketService.ClearBasketAsync(basketId);

            return Ok(result);
        }
    }
}
