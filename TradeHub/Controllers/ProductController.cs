using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using TradeHub.Errors;
using TradeHub.Helpers;
using TradeHub.Service.Products.Command.Create_Product;
using TradeHub.Service.Products.Command.Delete_product;
using TradeHub.Service.Products.Command.Update_Product;
using TradeHub.Service.Products.Queries;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Specifications.Product_Spec;

namespace TradeHub.Controllers
{
    [Authorize]
    public class ProductController : BaseApiController
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateProduct([FromForm] CreateProductDto Product)
        {
            var Result = await _mediator.Send(new CreateProductCommand(Product));
            return Ok(new ApiResponse(200, "Product added successfully"));
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(product);
        }
        [HttpGet]
        //[Cashed]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery] ProductSpecParams productSpecParams)
        {
            try
            {
                var result = await _mediator.Send(new GetProductsQuery(productSpecParams));
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<bool>> UpdateProduct(int id,[FromForm] UpdateProductDto productDto)
        {
            await _mediator.Send(new UpdateProductCommand { Id = id, ProductDto = productDto });
            return NoContent();
        }
        [HttpGet("random")]
        public async Task<ActionResult<List<ProductDto>>> GetRandomProducts([FromQuery] ProductSpecParams productSpec)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var products = await _mediator.Send(
                new GetRandomProductsQuery(productSpec)
                {
                    UserId = userId
                });
            if (products == null || products.Count == 0)
            {
                return NotFound(new ApiResponse(404, "No products found"));
            }
            return Ok(products);
        }
        [HttpGet("companies/{companyId}/subcategories")]
        [ProducesResponseType(typeof(IReadOnlyList<SubCategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSubCategoriesByCompanyId(string companyId)
        {
            var result = await _mediator.Send(new GetSubCategoriesByCompanyIdQuery(companyId));
            return Ok(result);
        }
        [HttpGet("by-subcategory/{subCategoryId}")]
        [ProducesResponseType(typeof(IReadOnlyList<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsBySubCategoryId(int subCategoryId)
        {
            var result = await _mediator.Send(new GetProductsBySubCategoryIdQuery(subCategoryId));

            return Ok(result);
        }
        [HttpPut("{id}/image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProductImage(
             int id,
             [FromForm] UpdateProductImageDto dto)
        {
            var result = await _mediator.Send(new UpdateProductImageCommand
            {
                ProductId = id,
                Image = dto.Image
            });

            if (result is null)
                return NotFound(new { message = "Product not found" });

            return Ok(new { imageUrl = result });
        }
    }
}
