using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using SupportTicketingBusiness.DTO;
using SupportTicketingBusiness.Services;

namespace SupportTicketingBusiness.Interface
{
    public interface IProductService
    {
        Task<ServiceResponse<List<ProductDto>>> GetAllAsync();
    }
}
