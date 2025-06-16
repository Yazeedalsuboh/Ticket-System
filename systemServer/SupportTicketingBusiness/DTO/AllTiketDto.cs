using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportTicketingData.Entities;
using SupportTicketingData.Enums;

namespace SupportTicketingBusiness.DTO
{
    public class AllTiketDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Priority { get; set; }
        public required TicketStatus Status { get; set; } 
        public required string ClientName { get; set; }
        public string? SupportName { get; set; }
        public required string ProductName { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
