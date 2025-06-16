using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicketingBusiness.DTO
{
    public class CommentDto
    {
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Username { get; set; }
    }
}
