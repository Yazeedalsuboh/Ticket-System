using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicketingBusiness.DTO
{
    public class TokenResponseDTO
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}
