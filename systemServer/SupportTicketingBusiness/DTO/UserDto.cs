using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportTicketingData.Enums;

namespace SupportTicketingBusiness.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public int TicketCount { get; set; }
        public bool IsActive { get; set; }
    }
}
