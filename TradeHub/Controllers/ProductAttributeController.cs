using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradeHub.Errors;
using TradHub.Core.Dtos;
using TradHub.Core.Service_Contract;

namespace TradeHub.Controllers
{
    public class ProductAttributeController : BaseApiController
    {
        private readonly IProductAttributeService _productAttributeService;

        public ProductAttributeController(IProductAttributeService productAttributeService)
        {
            _productAttributeService = productAttributeService;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<productattributedto>>> GetAllProductAttributes()
        {
            var productAttributes = await _productAttributeService.GetAllProductAttribute();
            if(productAttributes == null || !productAttributes.Any())
            {
                return NotFound(new ApiResponse(404, "No product attributes found."));
            }
            return Ok(productAttributes);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<productattributedto>> GetProductAttributeById(int id)
        {
            var productAttribute = await _productAttributeService.GetProductAttributeById(id);
            if(productAttribute is null)
            {
                return NotFound(new ApiResponse(404, $"Product attribute with ID {id} not found."));
            }
            return Ok(productAttribute);
        }
        [HttpPost]
        public async Task<ActionResult> CreateProductAttribute([FromBody] CreateProductAttribute dto)
        {
            var result = await _productAttributeService.CreateProductAttributesAsync(dto);
            if(!result)
            {
                return BadRequest(new ApiResponse(400, "Failed to create product attribute."));
            }
            return Ok("Product Attribute added Successfully");
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProductAttribute(int id, [FromBody] CreateProductAttribute dto)
        {
            var result = await _productAttributeService.UpdateProductAttributesAsync(id, dto);
            if(!result)
            {
                return BadRequest(new ApiResponse(400, $"Failed to update product attribute with ID {id}."));
            }
            return Ok("Product Attribute updated Successfully");
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductAttribute(int id)
        {
            var result = await _productAttributeService.DeleteProductAttributesAsync(id);
            if(!result)
            {
                return BadRequest(new ApiResponse(400, $"Failed to delete product attribute with ID {id}."));
            }
            return Ok("Product Attribute deleted Successfully");
        }
    }
}
