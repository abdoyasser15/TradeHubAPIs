using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TradeHub.Errors;
using TradeHub.Helpers;
using TradeHub.Service.Products.Command.Create_Product;
using TradeHub.Service.Products.Command.Delete_product;
using TradeHub.Service.Products.Command.Update_Product;
using TradeHub.Service.Products.Queries;
using TradHub.Core.Dtos;
using TradHub.Core.Specifications.Product_Spec;

namespace TradeHub.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductDto Product)
        {
            try
            {
                var result = await _mediator.Send(new CreateProductCommand(Product));
                return Ok(new ApiResponse(200, "Product added successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse(500, "Something went wrong"));
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            try
            {
                var product = await _mediator.Send(new GetProductByIdQuery(id));
                if (product is null)
                    return NotFound(new ApiResponse(404, $"Product with Id {id} not found"));

                return Ok(product);
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse(500, "Something went wrong"));
            }
        }
        [HttpGet]
        [Cashed]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery] ProductSpecParams productSpecParams)
        {
            try
            {
                var result = await _mediator.Send(new GetProductsQuery(productSpecParams));
                return Ok(result);
            }
            catch
            {
                return StatusCode(500, new ApiResponse(500, "Something went wrong"));
            }
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            try
            {
                var success = await _mediator.Send(new DeleteProductCommand(id));

                if (!success)
                    return NotFound(new { message = $"Product with Id {id} not found" });

                return Ok(new { message = "Product deleted successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse(500, "Something went wrong"));
            }
        }
        [HttpPut]
        public async Task<ActionResult<bool>> UpdateProduct(int id,[FromBody] UpdateProductDto productDto)
        {
            try
            {
                var updatedProduct = await _mediator.Send(new UpdateProductCommand { Id = id, ProductDto = productDto });
                return Ok(new ApiResponse(200, "Product Updated Successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse(500, "Something went wrong"));
            }
        }
    }
}
