using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportTicketingData.Enums;

namespace SupportTicketingBusiness.DTO
{
    public class GetUsersByRoleDto
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public UserRole? Role { get; set; }
        public string? Search { get; set; }
    }
}
