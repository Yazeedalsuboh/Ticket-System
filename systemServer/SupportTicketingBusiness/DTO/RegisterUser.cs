using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportTicketingData.Enums;

namespace SupportTicketingBusiness.DTO
{
    public class RegisterUser
    {
        public required string FullName { get; set; }
        public required string Image { get; set; }
        public string? Mobile { get; set; }
        public required string Email { get; set; }
        public string Password { get; set; }
        public required string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
