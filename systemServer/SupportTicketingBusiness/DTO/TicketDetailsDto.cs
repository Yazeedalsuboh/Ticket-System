using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportTicketingData.Enums;

namespace SupportTicketingBusiness.DTO
{
    public class TicketDetailsDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string? Priority { get; set; }
        public string ClientName { get; set; }
        public string SupportName { get; set; }
        public string ProductName { get; set; }

        public List<AttachmentDto> Attachments { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
