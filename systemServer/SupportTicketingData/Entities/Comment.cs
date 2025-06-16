using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicketingData.Entities
{
    public class Comment : EntityBase
    {
        public required string Message { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public Ticket Ticket { get; set; }
        public Users User { get; set; }
    }
}
