using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TradHub.Core.Service_Contract;

namespace TradeHub.Controllers
{
    [Authorize]
    public class NotificationsController : BaseApiController
    {
        private readonly INotificationServiceRealTime _notificationServiceRealTime;

        public NotificationsController(INotificationServiceRealTime notificationServiceRealTime)
        {
            _notificationServiceRealTime = notificationServiceRealTime;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notifications = await _notificationServiceRealTime
                .GetUserNotificationsAsync(userId!);

            return Ok(notifications);
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var count = await _notificationServiceRealTime.GetUnreadCountAsync(userId!);

            return Ok(new { count });
        }

        [HttpPut("{notificationId}/read")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _notificationServiceRealTime.MarkAsReadAsync(notificationId, userId!);

            return NoContent();
        }
    }
}
