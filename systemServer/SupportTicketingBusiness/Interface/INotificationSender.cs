using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicketingBusiness.Interface
{
    public interface INotificationSender
    {
        Task SendNotificationAsync(string userId, string message, string link);
    }
}
