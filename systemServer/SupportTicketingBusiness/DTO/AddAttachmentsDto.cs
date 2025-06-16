using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SupportTicketingBusiness.DTO
{
    public class AddAttachmentsDto
    {       
        public int Id { get; set; }
        public List<IFormFile>? Files { get; set; }

    }
}
