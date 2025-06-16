using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportTicketingData.Entities;

namespace SupportTicketingData.Interface
{
    public interface ICommentRepo : IGenericRepo<Comment>
    {
        Task<List<Comment>> GetCommentsByTicketIdAsync(int ticketId);
    }
}
