using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicketingData.Entities
{
    public class Notification : EntityBase
    {
        public string UserId { get; set; }      
        public string Message { get; set; }     
        public string Link { get; set; }        
        public bool IsRead { get; set; } = false;  
    }
}
