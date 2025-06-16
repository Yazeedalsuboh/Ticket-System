using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportTicketingBusiness.DTO;
using SupportTicketingBusiness.Services;
using SupportTicketingData.Enums;

namespace SupportTicketingBusiness.Interface
{
    public interface ITicketService
    {
        Task<ServiceResponse<string>> AddTicketAsync(AddTicketDto dto);
        Task <List<AllTiketDto>> GetAllTicketsAsync();
        Task<ServiceResponse<(List<AllTiketDto> Tickets, int TotalCount)>> GetAllTicketsWithFilterAsync(TicketFilterDto filter);
        Task<ServiceResponse<TicketDetailsDto>> GetTicketByIdAsync(int Id);
        Task<ServiceResponse<string>> AddAttachmentsToTicketAsync( AddAttachmentsDto dto);
        Task<ServiceResponse<string>> UpdateTicket(UpdateTicketDto dto);
        Task<ServiceResponse<string>> CloseTheTicket(int TicketId);
        Task<ServiceResponse<string>> AssignTicketAsync(AssignTicketDto Dto);
        Task<TicketStatsDto> GetTicketStatsAsync();
        Task<SupportTicketStatsDto> GetSupportTicketStatsAsync(int supportId);
        Task<ProductTicketStatsDto> GetProductTicketStatsAsync(int productId);
        // dashboard
        Task<List<SummaryDto>> GetSummaryAsync();
    }
}

