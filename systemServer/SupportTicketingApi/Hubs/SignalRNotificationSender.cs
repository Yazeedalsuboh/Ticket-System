using Microsoft.AspNetCore.SignalR;
using SupportTicketingBusiness.Interface;

namespace SupportTicketingApi.Hubs
{
    public class SignalRNotificationSender : INotificationSender
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRNotificationSender(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotificationAsync(string userId, string message, string link)
        {
            Console.WriteLine($"Sending notification to group: {userId}");
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", new { message, link, v = DateTime.UtcNow.ToString() });
            Console.WriteLine("Notification sent");
        }
    }
}
