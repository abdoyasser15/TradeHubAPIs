using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using TradeHub.Service;
using TradHub.Core.Service_Contract;

namespace TradeHub.Controllers
{
    public class PaymobWebhookController : BaseApiController
    {
        private readonly IPaymentGatewayService _paymentGatewayService;

        public PaymobWebhookController(IPaymentGatewayService paymentGatewayService)
        {
            _paymentGatewayService = paymentGatewayService;
        }
        [HttpGet("webhook")]
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook(CancellationToken cancellationToken)
        {
            Request.EnableBuffering();

            using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
            var rawBody = await reader.ReadToEndAsync(cancellationToken);
            Request.Body.Position = 0;

            await _paymentGatewayService.HandlePaymobWebhookAsync(
                rawBody,
                Request.Headers,
                Request.Query,
                cancellationToken);

            

            return Ok("Payment processed successfully");
        }
    }
}
