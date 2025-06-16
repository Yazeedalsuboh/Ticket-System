using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportTicketingBusiness.DTO;
using SupportTicketingBusiness.Services;
using SupportTicketingData.Entities;
using SupportTicketingData.Enums;

namespace SupportTicketingBusiness.Interface
{
    public interface IUserService
    {
        Task<List<SupportUserDto>> GetSupportUsersAsync();
        Task<List<SupportUserDto>> GetSupportUsersAsyncToDashboard();
        Task<(List<UserDto>, int TotalCount)> GetUsersByRoleAsync(int pageNumber,int pageSize, UserRole? Role, string? Search);
        Task<ServiceResponse<string>> ActivateUserAsync(int userId);
        Task<ServiceResponse<string>> DeactivateUserAsync(int userId);
        Task<SupportStatsDto> GetSupportStatisticsAsync();
    }      
}
