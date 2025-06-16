using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicketingData.Entities
{
    public class VerificationCode :EntityBase
    {       
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
