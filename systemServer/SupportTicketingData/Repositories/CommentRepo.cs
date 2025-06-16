using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SupportTicketingData.Data;
using SupportTicketingData.Entities;
using SupportTicketingData.Interface;

namespace SupportTicketingData.Repositories
{
    public class CommentRepo : GenericRepo<Comment>, ICommentRepo
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserRepo _currentUserRepo;

        public CommentRepo(AppDbContext context, ICurrentUserRepo currentUserRepo) : base(context)
        {
            _context = context;
            _currentUserRepo = currentUserRepo;
        }

        public async Task<List<Comment>> GetCommentsByTicketIdAsync(int ticketId)
        {
            return await _context.Comment
                .Where(c => c.TicketId == ticketId)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
