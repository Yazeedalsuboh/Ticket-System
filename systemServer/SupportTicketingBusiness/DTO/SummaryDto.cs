using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicketingBusiness.DTO
{
    public class SummaryDto
    {
        public required string Title { get; set; }
        public int Amount { get; set; }
    }
}
