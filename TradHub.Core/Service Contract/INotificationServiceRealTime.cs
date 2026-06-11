using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;

namespace TradHub.Core.Service_Contract
{
    public interface INotificationServiceRealTime
    {
        Task SendToUserAsync(
            string userId,
            string title,
            string message,
            string type
        );
        Task<List<Notification>> GetUserNotificationsAsync(string userId);
        Task MarkAsReadAsync(int notificationId, string userId);
        Task<int> GetUnreadCountAsync(string userId);
    }
}
