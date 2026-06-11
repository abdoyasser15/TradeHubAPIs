using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Hubs;
using TradeHub.Repository;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service
{
    public class NotificationServiceRealTime : INotificationServiceRealTime
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationServiceRealTime(AppDbContext context,IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;   
        }
        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId, string userId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification == null)
                throw new KeyNotFoundException("Notification not found");

            notification.IsRead = true;

            await _context.SaveChangesAsync();
        }

        public async Task SendToUserAsync(
            string userId,
            string title,
            string message,
            string type)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            await _hubContext.Clients
                .Group($"user-{userId}")
                .SendAsync("ReceiveNotification", new
                {
                    notification.Id,
                    notification.Title,
                    notification.Message,
                    notification.Type,
                    notification.IsRead,
                    notification.CreatedAt
                });
        }
    }
}
