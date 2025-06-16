using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SupportTicketingData.Enums;

namespace SupportTicketingData.Entities
{
    public class Ticket : EntityBase
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required TicketStatus Status { get; set; } = TicketStatus.Open;
        public string? Priority { get; set; }
        public int ClientId { get; set; }
        public int? SupportId { get; set; }
        public int ProductId { get; set; }
        public Users Client {  get; set; }
        public Users Support { get; set; }
        public Product Product { get; set; }
        public List<Comment> Comments { get; set; } = new();
        public List<Attachment> Attachments { get; set; } = new();
    }
}
