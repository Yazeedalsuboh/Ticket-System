using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SupportTicketingBusiness.DTO
{
    public class SupportUserDto
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
    }
}
