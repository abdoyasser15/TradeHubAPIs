using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TradeHub.Repository;
using TradeHub.Service;
using TradHub.Core.Dtos;
using TradHub.Core.Specifications.OrderSpec;

namespace TradeHub.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly AppDbContext _context;
        private readonly IPaymentGatewayService _paymentGateway;

        public OrdersController(
            IOrderService orderService,
            AppDbContext context,
            IPaymentGatewayService paymentGateway)
        {
            _orderService = orderService;
            _context = context;
            _paymentGateway = paymentGateway;
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CreatePaymentSessionResponse>> Checkout(
            [FromBody] CreateOrderFromBasketRequest request,
            CancellationToken cancellationToken)
        {
            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(buyerId))
                return Unauthorized();

            var orderDto = await _orderService.CreateOrderFromBasketAsync(buyerId, request, cancellationToken);

            var order = await _context.Orders
                .Include(x => x.Items)
                .FirstAsync(x => x.Id == orderDto.Id, cancellationToken);

            var session = await _paymentGateway.CreatePaymobSessionAsync(order, cancellationToken);

            return Ok(session);
        }
        [HttpGet]
        public async Task<ActionResult<Pagination<OrderDto>>> GetOrders(
             [FromQuery] OrderSpecParams specParams,
             CancellationToken cancellationToken)
        {
            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(buyerId))
                return Unauthorized();

            var orders = await _orderService.GetOrdersAsync(
                buyerId,
                specParams,
                cancellationToken);

            return Ok(orders);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id, CancellationToken cancellationToken)
        {
            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(buyerId))
                return Unauthorized();

            var order = await _orderService.GetOrderByIdAsync(id, buyerId, cancellationToken);
            if (order is null)
                return NotFound();

            return Ok(order);
        }
    }
}
