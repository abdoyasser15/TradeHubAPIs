using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TradeHub.Repository;
using TradHub.Core.Dtos;
using TradHub.Core.Entity.Orders;
using TradHub.Core.Entity.Payments;
using TradHub.Core.Enums;
using TradHub.Core.Service_Contract;
using TradHub.Core.Settings;

namespace TradeHub.Service
{
    public class PaymobPaymentGatewayService : IPaymentGatewayService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;
        private readonly INotificationServiceRealTime _notificationServiceRealTime;
        private readonly PaymobSettings _settings;

        public PaymobPaymentGatewayService(
            HttpClient httpClient,
            AppDbContext context,
            IOptions<PaymobSettings> options, INotificationServiceRealTime notificationServiceRealTime)
        {
            _httpClient = httpClient;
            _context = context;
            _notificationServiceRealTime = notificationServiceRealTime;
            _settings = options.Value;

            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<CreatePaymentSessionResponse> CreatePaymobSessionAsync(
            Order order,
            CancellationToken cancellationToken = default)
        {
            var items = order.Items.Select(x => new
            {
                name = x.ProductName,
                amount = (int)Math.Round(x.Price * 100),
                quantity = x.Quantity
            }).ToArray();

            var totalAmountCents = items.Sum(x => x.amount * x.quantity);

            var body = new
            {
                amount = totalAmountCents,
                currency = _settings.Currency,
                payment_methods = new[] { _settings.IntegrationId },
                items,

                billing_data = new
                {
                    apartment = "NA",
                    first_name = "Test",
                    last_name = "User",
                    street = "NA",
                    building = "NA",
                    phone_number = "01000000000",
                    country = "EG",
                    email = "test@test.com",
                    floor = "NA",
                    state = "NA",
                    city = "Cairo"
                },

                customer = new
                {
                    first_name = "Test",
                    last_name = "User",
                    email = "test@test.com",
                    phone_number = "01000000000"
                },

                merchant_order_id = order.Id.ToString(),

                extras = new
                {
                    order_id = order.Id
                },

                notification_url = _settings.CallbackUrl,
                redirection_url = _settings.ReturnUrl
            };

            using var req = new HttpRequestMessage(HttpMethod.Post, "/v1/intention/");
            req.Headers.Authorization =
                new AuthenticationHeaderValue("Token", _settings.SecretKey);

            req.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");

            using var response = await _httpClient.SendAsync(req, cancellationToken);
            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Paymob intention creation failed: {json}");

            using var doc = JsonDocument.Parse(json);

            var paymentIntentId = doc.RootElement.GetProperty("id").ToString();

            var clientSecret = doc.RootElement.TryGetProperty("client_secret", out var cs)
                ? cs.GetString()!
                : string.Empty;

            order.PaymentIntentId = paymentIntentId;
            order.PaymentOrderReference = order.Id.ToString();
            order.OrderStatus = OrderStatus.AwaitingPayment;
            order.PaymentStatus = PaymentStatus.Pending;

            await _context.SaveChangesAsync(cancellationToken);

            return new CreatePaymentSessionResponse
            {
                OrderId = order.Id,
                PaymentIntentId = paymentIntentId,
                ClientSecret = clientSecret,
                PublicKey = _settings.PublicKey,
                PaymentUrl =
                    $"https://accept.paymob.com/unifiedcheckout/?publicKey={_settings.PublicKey}&clientSecret={clientSecret}"
            };
        }

        public async Task HandlePaymobWebhookAsync(
            string rawBody,
            IHeaderDictionary headers,
            IQueryCollection query,
            CancellationToken cancellationToken = default)
        {
            bool success;
            bool pending;
            string? transactionId;
            string? orderIdStr = null;
            string? cardLastDigits = null;

            if (!string.IsNullOrWhiteSpace(rawBody))
            {
                using var doc = JsonDocument.Parse(rawBody);
                var root = doc.RootElement;

                var data = root.TryGetProperty("obj", out var objProp)
                    ? objProp
                    : root;

                transactionId = data.TryGetProperty("id", out var idProp)
                    ? idProp.ToString()
                    : null;

                success = data.TryGetProperty("success", out var successProp)
                          && successProp.GetBoolean();

                pending = data.TryGetProperty("pending", out var pendingProp)
                          && pendingProp.GetBoolean();

                if (data.TryGetProperty("source_data", out var sourceData) &&
                    sourceData.TryGetProperty("pan", out var panProp))
                {
                    cardLastDigits = panProp.ToString();
                }

                if (data.TryGetProperty("merchant_order_id", out var merchantProp))
                    orderIdStr = merchantProp.ToString();

                if (string.IsNullOrWhiteSpace(orderIdStr) &&
                    data.TryGetProperty("extras", out var extrasProp) &&
                    extrasProp.TryGetProperty("order_id", out var orderIdProp))
                {
                    orderIdStr = orderIdProp.ToString();
                }

                if (string.IsNullOrWhiteSpace(orderIdStr) &&
                    data.TryGetProperty("order", out var orderProp))
                {
                    if (orderProp.ValueKind == JsonValueKind.Object &&
                        orderProp.TryGetProperty("id", out var paymobOrderIdProp))
                    {
                        orderIdStr = paymobOrderIdProp.ToString();
                    }
                    else
                    {
                        orderIdStr = orderProp.ToString();
                    }
                }
            }
            else
            {
                transactionId = query["id"].FirstOrDefault();
                success = query["success"].FirstOrDefault()?.ToLower() == "true";
                pending = query["pending"].FirstOrDefault()?.ToLower() == "true";

                orderIdStr =
                    query["merchant_order_id"].FirstOrDefault()
                    ?? query["order_id"].FirstOrDefault()
                    ?? query["order"].FirstOrDefault();

                cardLastDigits = query["source_data.pan"].FirstOrDefault();
            }

            Order? order = null;

            if (int.TryParse(orderIdStr, out var orderId))
            {
                order = await _context.Orders
                    .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);
            }

            if (order is null)
            {
                throw new Exception(
                    $"Order not found. orderIdStr={orderIdStr}, transactionId={transactionId}, rawBody={rawBody}");
            }

            var paymentStatus = MapPaymentStatus(success, pending);

            order.PaymentTransactionId = transactionId;
            order.PaymentStatus = paymentStatus;

            if (!string.IsNullOrWhiteSpace(cardLastDigits))
            {
                order.CardLast4Digits = cardLastDigits;
            }

            switch (paymentStatus)
            {
                case PaymentStatus.Paid:
                    order.OrderStatus = OrderStatus.Confirmed;
                    order.PaidAt = DateTime.UtcNow;
                    break;

                case PaymentStatus.Pending:
                    order.OrderStatus = OrderStatus.AwaitingPayment;
                    break;

                case PaymentStatus.Failed:
                    order.OrderStatus = OrderStatus.Failed;
                    break;

                case PaymentStatus.Cancelled:
                    order.OrderStatus = OrderStatus.Cancelled;
                    order.CancelledAt = DateTime.UtcNow;
                    break;

                case PaymentStatus.Refunded:
                case PaymentStatus.PartiallyRefunded:
                    order.OrderStatus = OrderStatus.Refunded;
                    order.RefundedAt = DateTime.UtcNow;
                    break;
            }

            _context.PaymentTransactions.Add(new PaymentTransaction
            {
                OrderId = order.Id,
                Provider = "Paymob",
                ProviderTransactionId = transactionId,
                ProviderIntentId = order.PaymentIntentId,
                ProviderOrderReference = orderIdStr,
                PaymentStatus = paymentStatus,
                IsPending = pending,
                IsSuccess = success,
                RawPayload = string.IsNullOrWhiteSpace(rawBody)
                    ? string.Join("&", query.Select(q => $"{q.Key}={q.Value}"))
                    : rawBody
            });

            await _context.SaveChangesAsync(cancellationToken);

            if (paymentStatus is PaymentStatus.Paid)
            {
                await _notificationServiceRealTime.SendToUserAsync(
                    order.BuyerId,
                    "Payment Success",
                    $"Payment for order #{order.Id} completed successfully",
                    $"{order.PaymentStatus.ToString()}"
                );
            }
        }

        private PaymentStatus MapPaymentStatus(bool success, bool pending)
        {
            if (pending) return PaymentStatus.Pending;
            if (success) return PaymentStatus.Paid;

            return PaymentStatus.Failed;
        }

        private bool IsValidHmac(string rawBody, IHeaderDictionary headers)
        {
            var receivedHmac = headers["HMAC"].FirstOrDefault()
                               ?? headers["X-HMAC"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(receivedHmac))
                return false;

            using var hmac =
                new HMACSHA512(Encoding.UTF8.GetBytes(_settings.HmacSecret));

            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawBody));
            var computed = BitConverter
                .ToString(hash)
                .Replace("-", "")
                .ToLowerInvariant();

            return computed == receivedHmac.ToLowerInvariant();
        }
    }
}