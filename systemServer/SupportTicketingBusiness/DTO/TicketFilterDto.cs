using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportTicketingData.Enums;

namespace SupportTicketingBusiness.DTO
{
    public class TicketFilterDto
    {
        public TicketStatus? Status { get; set; }
        public string? SearchTitle { get; set; }
        public int? ProductId { get; set; }
        public UserRole? UserRoleFilter { get; set; } // "Client" or "Support"
        public int? UserIdFilter { get; set; } 
        public bool SortByCreatedAtAsc { get; set; } = false; 
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
