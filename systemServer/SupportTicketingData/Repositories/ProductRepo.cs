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
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        private readonly AppDbContext _context;
        public ProductRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Products.CountAsync();
        }
    }
}
