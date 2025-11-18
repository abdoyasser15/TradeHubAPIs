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
            try
            {
                var productAttributes = await _productAttributeService.GetAllProductAttribute();
                if (productAttributes == null || !productAttributes.Any())
                {
                    return NotFound(new ApiResponse(404, "No product attributes found."));
                }
                return Ok(productAttributes);
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse(500, "Something went wrong"));
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<productattributedto>> GetProductAttributeById(int id)
        {
            try
            {
                var productAttribute = await _productAttributeService.GetProductAttributeById(id);
                if (productAttribute is null)
                {
                    return NotFound(new ApiResponse(404, $"Product attribute with ID {id} not found."));
                }
                return Ok(productAttribute);
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse(500, "Something went wrong"));
            }
        }
        [HttpPost]
        public async Task<ActionResult> CreateProductAttribute([FromBody] CreateProductAttribute dto)
        {
            try
            {
                var result = await _productAttributeService.CreateProductAttributesAsync(dto);
                if (!result)
                {
                    return BadRequest(new ApiResponse(400, "Failed to create product attribute."));
                }
                return Ok("Product Attribute added Successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse(500, "Something went wrong"));
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProductAttribute(int id, [FromBody] CreateProductAttribute dto)
        {
            try
            {
                var result = await _productAttributeService.UpdateProductAttributesAsync(id, dto);
                if (!result)
                {
                    return BadRequest(new ApiResponse(400, $"Failed to update product attribute with ID {id}."));
                }
                return Ok("Product Attribute updated Successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse(500, "Something went wrong"));
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductAttribute(int id)
        {
            try
            {
                var result = await _productAttributeService.DeleteProductAttributesAsync(id);
                if (!result)
                {
                    return BadRequest(new ApiResponse(400, $"Failed to delete product attribute with ID {id}."));
                }
                return Ok("Product Attribute deleted Successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse(500, "Something went wrong"));
            }
        }
    }
}
