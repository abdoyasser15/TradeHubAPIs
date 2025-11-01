using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradeHub.Errors;
using TradeHub.Service.Products.Command.Create_Product;
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
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery] ProductSpecParams productSpecParams)
        {
            var products = await _mediator.Send(new GetProductsQuery(productSpecParams));
            if (products == null)
                return NotFound(new ApiResponse(404, "Products Not Found"));
            return Ok(products);
        }
    }
}
