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
    public class VerificationCodeRepo : GenericRepo<VerificationCode>, IVerificationCodeRepo
    {
        private readonly AppDbContext _context;
        public VerificationCodeRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> Verify(string email, string code)
        {
            var record = await _context.VerificationCode
                .Where(v => v.Email == email && v.Code == code && !v.IsUsed && v.ExpiryTime > DateTime.UtcNow)
                .FirstOrDefaultAsync();

            if (record == null)
                return false;

            record.IsUsed = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
