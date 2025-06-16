using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using SupportTicketingData.Data;
using SupportTicketingData.Entities;
using SupportTicketingData.Enums;
using SupportTicketingData.Interface;

namespace SupportTicketingData.Repositories
{
    public class UserRepo : GenericRepo<Users>, IUserRepo
    {
        private readonly AppDbContext _context;
        public UserRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Users>> GetSupportUsersAsync()
        {
            return await _context.Users
                .Where(u => u.Role == UserRole.Support && u.IsActive == true)
                .ToListAsync();

        }
        public async Task<int> GetClientsUsersAsync()
        {
            return await _context.Users.CountAsync(u => u.Role == UserRole.Client);
        }
        public async Task<int> GetSupportsUsersAsync()
        {
            return await _context.Users.CountAsync(u => u.Role == UserRole.Support);
        }
        public async Task<List<Users>> GetSupportUsersAsyncToDashboard()
        {
            return await _context.Users
                .Where(u => u.Role == UserRole.Support)
                .Include(u => u.SupportTickets)
                .ToListAsync();
        }
        public async Task<(List<Users> ,int )> GetUsersWithFiltersAsync(int pageNumber,
            int pageSize ,UserRole? Role ,string? Search)
        {
            var query = _context.Users.AsQueryable();

            if (Role != null)
                query = query.Where(u => u.Role == Role);

            if (!string.IsNullOrWhiteSpace(Search))
            {
                query = query.Where(u =>
                    u.FullName.Contains(Search) ||
                    u.Mobile.Contains(Search) ||
                    u.Email.Contains(Search));
            }
            query = query.Include(u => u.ClientTickets)
                         .Include(u => u.SupportTickets);
            var TotalCount = await query.CountAsync();

            query = query.Skip((pageNumber) * pageSize)
                 .Take(pageSize);
            var result = await query.ToListAsync();
            return (result , TotalCount);
        }

        public async Task<(int total, int active)> GetSupportCountsAsync()
        {
            var total = await _context.Users.CountAsync(u => u.Role == UserRole.Support);
            var active = await _context.Users.CountAsync(u => u.Role == UserRole.Support && u.IsActive);
            return (total, active);
        }

        public async Task<Users?> GetByIdWithTicketsAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.ClientTickets)
                .Include(u => u.SupportTickets)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Users.CountAsync();
        }
    }
}
