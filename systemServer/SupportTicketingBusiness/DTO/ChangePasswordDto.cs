using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicketingBusiness.DTO
{
    public class ChangePasswordDto
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
