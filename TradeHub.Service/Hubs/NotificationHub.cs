using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TradeHub.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        override public async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"user-{userId}"
                );
            }

            await base.OnConnectedAsync();
        }
    }
}
