using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SupportTicketingApi.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst("sub")?.Value; // استخرج الـ sub من التوكن الموثّق
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Group_{userId}");
                Console.WriteLine($"Client {Context.ConnectionId} added to Group_{userId}");
            }
            else
            {
                Console.WriteLine($"Failed to add client {Context.ConnectionId}: No userId found");
            }
            await base.OnConnectedAsync();
        }
    }
}