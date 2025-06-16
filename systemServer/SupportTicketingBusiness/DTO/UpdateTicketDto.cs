using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SupportTicketingBusiness.DTO
{
    public class UpdateTicketDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public  string? Priority { get; set; }
        public required string Description { get; set; }
        public required string ProductName { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
