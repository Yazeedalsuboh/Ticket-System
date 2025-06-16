using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SupportTicketingData.Entities;
using SupportTicketingData.Enums;

namespace SupportTicketingData.Interface
{
    public interface IUserRepo : IGenericRepo<Users>
    {
        Task<List<Users>> GetSupportUsersAsync();
        Task<int> GetClientsUsersAsync();
        Task<int> GetSupportsUsersAsync();
        Task<List<Users>> GetSupportUsersAsyncToDashboard();
        Task<(List<Users>, int)> GetUsersWithFiltersAsync(int pageNumber,int pageSize, UserRole? Role, string? Search);
        Task<(int total, int active)> GetSupportCountsAsync();
        Task<Users?> GetByIdWithTicketsAsync(int userId);
        Task<int> GetTotalCountAsync();

    }
}
