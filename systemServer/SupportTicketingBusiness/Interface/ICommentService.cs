using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportTicketingBusiness.DTO;
using SupportTicketingBusiness.Services;

namespace SupportTicketingBusiness.Interface
{
    public interface ICommentService
    {
        Task<ServiceResponse<bool>> AddCommentAsync(AddCommentDto dto);
        Task<ServiceResponse<List<CommentDto>>> GetCommentsByTicketIdAsync(int ticketId);
    }
}
