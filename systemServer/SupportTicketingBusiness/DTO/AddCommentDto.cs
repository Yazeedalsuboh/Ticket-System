using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicketingBusiness.DTO
{
    public class AddCommentDto
    {
        public required string Message { get; set; }
        public int Id { get; set; }
    }
}
