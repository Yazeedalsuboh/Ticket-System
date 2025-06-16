using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicketingBusiness.DTO
{
    public class TicketStatsDto
    {
        public int Opened { get; set; }
        public int Assigned { get; set; }
        public int Closed { get; set; }
    }
}
