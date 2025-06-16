using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicketingData.Entities
{
    public class Attachment : EntityBase
    {
        public required string FilePath { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket {  get; set; }
    }
}
