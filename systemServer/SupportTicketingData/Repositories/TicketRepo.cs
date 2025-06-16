using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NPOI.SS.Formula.Functions;
using SupportTicketingData.Data;
using SupportTicketingData.Entities;
using SupportTicketingData.Enums;
using SupportTicketingData.Interface;

namespace SupportTicketingData.Repositories
{
    public class TicketRepo: GenericRepo<Ticket> , ITicketRepo  
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserRepo _currentUserRepo;
        public TicketRepo(AppDbContext context, ICurrentUserRepo currentUserRepo) : base(context)
        {
            _context = context;
            _currentUserRepo = currentUserRepo;
        }
        public async Task<List<Ticket>> GetAllWithDetailsAsync()
        {
            var userId = _currentUserRepo.GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException("User is not authenticated.");
            var role = _context.Users
                           .Where(u => u.Id == userId)
                           .Select(u => u.Role) 
                           .FirstOrDefault();
            if (role == null)
                throw new UnauthorizedAccessException("User does not have a role.");

            IQueryable<Ticket> query = _context.Ticket.Include(t => t.Client)
                                                 .Include(t => t.Support)
                                                 .Include(t => t.Product);

            if (role == UserRole.Manager)
            {
                return await query.ToListAsync();
            }

            
            if (role == UserRole.Support)
            {
                return await query.Where(t => t.SupportId == userId).ToListAsync();
            }

            
            if (role == UserRole.Client)
            {
                return await query.Where(t => t.ClientId == userId).ToListAsync();
            }

            throw new UnauthorizedAccessException("User role is not recognized.");
        }
        public async Task<(List<Ticket> , int )> GetAllWithFiltersAsync(int pageNumber,
            int pageSize,
            TicketStatus? status,
            UserRole? userRoleFilter,
            string? searchTitle = null, int? productId = null,
            int? userIdFilter = null , 
            bool sortByCreatedAtAsc = false)
        {
            var userId = _currentUserRepo.GetUserId();
            if (userId == null)
                return (new List<Ticket>(), 0);

            var role = await _context.Users
                               .AsNoTracking()
                               .Where(u => u.Id == userId)
                               .Select(u => u.Role)                               
                               .FirstOrDefaultAsync();

            if (role == null)
                return (new List<Ticket>(), 0);

            IQueryable<Ticket> query = _context.Ticket
                                               .Include(t => t.Client)
                                               .Include(t => t.Support)
                                               .Include(t => t.Product);

            if (status.HasValue)  //Status 
            {
                query = query.Where(t => t.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchTitle))  //Title 
            {
                var lowerSearch = searchTitle.ToLower();
                query = query.Where(t => t.Title.ToLower().Contains(lowerSearch));
            }

            if (productId.HasValue)  //Product
            {
                query = query.Where(t => t.ProductId == productId.Value);
            }
             //Current user
            if (role == UserRole.Support)
            {
                query = query.Where(t => t.SupportId == userId);
            }
            else if (role == UserRole.Client)
            {
                query = query.Where(t => t.ClientId == userId);
            }
            else 
            {                
                if (userRoleFilter.HasValue)
                {
                    if (userRoleFilter == UserRole.Support)
                    {
                        query = query.Where(t => t.SupportId != null);
                    }
                }
                if (userIdFilter.HasValue)
                {
                    query = query.Where(t => t.ClientId == userIdFilter.Value || t.SupportId == userIdFilter.Value);
                }
            }

            query = sortByCreatedAtAsc
                  ? query.OrderBy(t => t.CreatedAt)
                  : query.OrderByDescending(t => t.CreatedAt);

            var result =await query.Skip((pageNumber) * pageSize)
                  . Take(pageSize) .ToListAsync();
            var total = await query.CountAsync();
            return (result , total);
        }
        public async Task<Ticket?> GetByIdWithAllDetailsAsync(int id)
        {
            return await _context.Ticket
                                 .Include(t => t.Client)
                                 .Include(t => t.Support)
                                 .Include(t => t.Product)
                                 .Include(t => t.Attachments)
                                 .Include(t => t.Comments)
                                 .ThenInclude(c => c.User)
                                 .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Ticket?> GetTicketWithAttachmentsAsync(int ticketId)
        {
            return await _context.Ticket
                .Include(t => t.Attachments)                             
                .FirstOrDefaultAsync(t => t.Id == ticketId);
        }

        public async Task<List<Ticket>> GetTicketsBySupportIdAsync(int supportId)
        {
            return await _context.Ticket
                .Where(t => t.SupportId == supportId)
                .ToListAsync();
        }
        public async Task<List<Ticket>> GetTicketsByProductIdAsync(int productId)
        {
            return await _context.Ticket
                .Where(t => t.ProductId == productId)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Ticket.CountAsync();
        }
    }
}
