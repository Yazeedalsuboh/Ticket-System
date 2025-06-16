using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SupportTicketingData.Entities;
using SupportTicketingData.Enums;

namespace SupportTicketingData.Interface
{
    public interface ITicketRepo : IGenericRepo<Ticket>
    {
        Task<List<Ticket>> GetAllWithDetailsAsync();
        Task<(List<Ticket>, int)> GetAllWithFiltersAsync(int pageNumber, int pageSize,
                                                   TicketStatus? status,
                                                   UserRole? userRoleFilter,
                                                   string? searchTitle = null, int? productId = null,
                                                   int? userIdFilter = null,
                                                   bool sortByCreatedAtAsc = false);       
        Task<Ticket?> GetByIdWithAllDetailsAsync(int id);
        Task<Ticket?> GetTicketWithAttachmentsAsync(int ticketId);
        Task<List<Ticket>> GetTicketsBySupportIdAsync(int supportId);
        Task<List<Ticket>> GetTicketsByProductIdAsync(int productId);
        Task<int> GetTotalCountAsync();        
    }
}
