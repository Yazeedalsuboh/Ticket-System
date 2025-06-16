using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportTicketingBusiness.Interface;
using SupportTicketingData.Entities;
using SupportTicketingData.Interface;

namespace SupportTicketingBusiness.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IGenericRepo<Notification> _notificationRepo;
        private readonly INotificationSender _notificationSender;

        public NotificationService(IGenericRepo<Notification> notificationRepo, INotificationSender notificationSender)
        {
            _notificationRepo = notificationRepo;
            _notificationSender = notificationSender;
        }

        public async Task CreateAndSendNotificationAsync(string userId, string message, string link)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Link = link,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
            await _notificationRepo.AddAsync(notification);
            await _notificationSender.SendNotificationAsync(userId, message, link);
        }
    }
}
