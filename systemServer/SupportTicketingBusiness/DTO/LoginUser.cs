using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicketingBusiness.DTO
{
    public class LoginUser
    {
        public required string Identifier { get; set; }
        public required string Password { get; set; }
    }
}
