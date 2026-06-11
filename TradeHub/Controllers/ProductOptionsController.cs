using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradHub.Core.Dtos;
using TradHub.Core.Service_Contract;

namespace TradeHub.Controllers
{
    public class ProductOptionsController : BaseApiController
    {
        private readonly IProductOptionsService _productOptionsService;

        public ProductOptionsController(IProductOptionsService productOptionsService)
        {
            _productOptionsService = productOptionsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetOptionsByProductId(int productId)
        {
            var options = await _productOptionsService.GetOptionsByProductIdAsync(productId);
            return Ok(options);
        }
        [HttpPost]
        public async Task<IActionResult> AddOptionToProduct(int productId, CreateProductOptionDto dto)
        {
            var option = await _productOptionsService.AddOptionToProductAsync(productId, dto);
            if (option == null)
                return BadRequest("Failed to add option to product.");
            return Ok(option);
        }
        [HttpDelete("{optionId}")]
        public async Task<IActionResult> DeleteOption(int optionId)
        {
            var success = await _productOptionsService.DeleteOptionAsync(optionId);
            if (!success)
                return NotFound("Option not found.");
            return NoContent();
        }
    }
}
