using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var product = await _mediator.Send(new CreateProductCommand(Product));
            if (product == null)
                return BadRequest(new ApiResponse(400, "Problem Creating Product"));
            return Ok("Product Added Successfully");
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if (product == null)
                return NotFound(new ApiResponse(404, "Product Not Found"));
            return Ok(product);
        }
        [HttpGet]
        [Cashed]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery] ProductSpecParams productSpecParams)
        {
            var products = await _mediator.Send(new GetProductsQuery(productSpecParams));
            if (products == null)
                return NotFound(new ApiResponse(404, "Products Not Found"));
            return Ok(products);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));
            if (!result)
                return NotFound(new ApiResponse(404, "Product Not Found"));
            return Ok(new ApiResponse(200, "Product Deleted Successfully"));
        }
        [HttpPut]
        public async Task<ActionResult<bool>> UpdateProduct(int id,[FromBody] UpdateProductDto productDto)
        {
            var updatedProduct = await _mediator.Send(new UpdateProductCommand { Id = id,ProductDto=productDto});
            if (updatedProduct == null)
                return BadRequest(new ApiResponse(400, "Problem Updating Product"));
            return Ok(new ApiResponse(200,"Product Updated Successfully"));
        }
    }
}
