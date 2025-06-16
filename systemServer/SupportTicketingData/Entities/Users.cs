using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using SupportTicketingData.Enums;

namespace SupportTicketingData.Entities
{
    public class Users : EntityBase
    {
        public required string FullName { get; set; }
        public string? Image {  get; set; }
        public  string? Mobile {  get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public required string Address { get; set; }
        public DateTime  DateOfBirth { get; set; }
        public List<Ticket> ClientTickets { get; set; } = new();
        public List<Ticket> SupportTickets { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();

        //For Tokens
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
