using Microsoft.EntityFrameworkCore;
using TradeHub.Repository;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity.Orders;
using TradHub.Core.Enums;
using TradHub.Core.Service_Contract;
using TradHub.Core.Specifications.OrderSpec;

namespace TradeHub.Service
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly INotificationServiceRealTime _notificationServiceRealTime;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(AppDbContext context, 
            INotificationServiceRealTime notificationServiceRealTime , IUnitOfWork unitOfWork)
        {
            _context = context;
            _notificationServiceRealTime = notificationServiceRealTime;
            _unitOfWork = unitOfWork;
        }
        public async Task<OrderDto> CreateOrderFromBasketAsync(
            string buyerId,
            CreateOrderFromBasketRequest request,
            CancellationToken cancellationToken = default)
        {
            var basket = await _context.Baskets
                         .Include(x => x.Items)
                             .ThenInclude(i => i.Options)
                         .FirstOrDefaultAsync(x =>
                             x.Id == request.BasketId &&
                             x.BuyerId == buyerId,
                             cancellationToken);

            if (basket is null || !basket.Items.Any())
                throw new Exception("Basket is empty.");

            var productIds = basket.Items.Select(x => x.ProductId).ToList();

            var products = await _context.Products
                .Include(x => x.Company)
                .Where(x => productIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (!products.Any())
                throw new Exception("Products not found.");

            var firstProduct = products.First();

            var order = new Order
            {
                BuyerId = buyerId,

                CompanyId = firstProduct.CompanyId,
                CompanyName = firstProduct.Company?.BusinessName ?? string.Empty,
                CompanyLogoUrl = firstProduct.Company?.LogoUrl ?? string.Empty,

                DeliveryFee = request.DeliveryFee,
                OrderStatus = OrderStatus.AwaitingPayment,
                PaymentStatus = PaymentStatus.Pending,
                Address = request.Address!,

                Items = new List<OrderItem>()
            };

            foreach (var basketItem in basket.Items)
            {
                var product = products.FirstOrDefault(x => x.Id == basketItem.ProductId);

                if (product is null)
                    throw new Exception($"Product {basketItem.ProductId} not found.");

                if (product.Quantity < basketItem.Quantity)
                    throw new Exception($"Insufficient stock for product {product.Name}.");

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name ?? string.Empty,
                    ImageUrl = product.ImageUrl ?? string.Empty,
                    Price = basketItem.Price,
                    Quantity = basketItem.Quantity,
                    CompanyId = product.CompanyId.ToString(),

                    Options = basketItem.Options.Select(o => new OrderItemOption
                    {
                        ProductOptionValueId = o.ProductOptionValueId,
                        OptionName = o.OptionName,
                        ValueName = o.ValueName,
                        ExtraPrice = o.ExtraPrice
                    }).ToList()
                });

                product.Quantity -= basketItem.Quantity;
            }

            order.SubTotal = order.Items.Sum(x => x.Price * x.Quantity);
            order.Total = order.SubTotal + order.DeliveryFee;

            _context.Orders.Add(order);

            _context.BasketItems.RemoveRange(basket.Items);
            _context.Baskets.Remove(basket);

            await _context.SaveChangesAsync(cancellationToken);

            var savedOrder = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == order.Id, cancellationToken);

            await _notificationServiceRealTime.SendToUserAsync(
                buyerId,
                "OrderCreated",
                $"Your order has been created successfully with OrderID: {order.Id}",
                $"Order Status: {order.PaymentStatus.ToString()}"
               );

            return MapToDto(savedOrder!);
        }
        public async Task<Pagination<OrderDto>> GetOrdersAsync(
            string buyerId,
            OrderSpecParams specParams,
            CancellationToken cancellationToken = default)
        {
            var spec = new OrderSpecification(buyerId, specParams);

            var orders = await _unitOfWork.Repository<Order>()
                .GetAllSpecificationsAsync(spec);

            var countSpec = new OrderSpecification(buyerId);

            var count = await _unitOfWork.Repository<Order>()
                .CountAsync(countSpec);

            var data = orders.Select(MapToDto).ToList();

            return new Pagination<OrderDto>(
                specParams.pageIndex,
                specParams.PageSize,
                count,
                data
            );
        }
        public async Task<OrderDto?> GetOrderByIdAsync(
            int orderId,
            string buyerId,
            CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders
                        .AsNoTracking()
                        .Include(x => x.Items)
                            .ThenInclude(i => i.Options)
                        .FirstOrDefaultAsync(x =>
                            x.Id == orderId &&
                            x.BuyerId == buyerId,
                            cancellationToken);

            return order is null ? null : MapToDto(order);
        }
        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                SubTotal = order.SubTotal,
                DeliveryFee = order.DeliveryFee,
                Total = order.Total,
                OrderStatus = order.OrderStatus.ToString(),
                PaymentStatus = order.PaymentStatus.ToString(),
                CreatedAt = order.CreatedAt,

                MaskedCardNumber = string.IsNullOrWhiteSpace(order.CardLast4Digits)
                 ? null
                    : $"**** **** **** {order.CardLast4Digits}",
                CompanyName = order.CompanyName,
                Address = order.Address,
                CompanyLogo = order.CompanyLogoUrl,
                Items = order.Items.Select(x => new OrderItemDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ImageUrl = x.ImageUrl,
                    Price = x.Price,
                    Quantity = x.Quantity,

                    Options = x.Options.Select(o => new OrderItemOptionDto
                    {
                        ProductOptionValueId = o.ProductOptionValueId,
                        OptionName = o.OptionName,
                        ValueName = o.ValueName,
                        ExtraPrice = o.ExtraPrice
                    }).ToList()
                }).ToList()
            };
        }
    }
}