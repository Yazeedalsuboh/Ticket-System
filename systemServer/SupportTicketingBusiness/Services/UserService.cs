using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SupportTicketingBusiness.DTO;
using SupportTicketingBusiness.Interface;
using SupportTicketingData.Enums;
using SupportTicketingData.Interface;
using SupportTicketingData.Repositories;

namespace SupportTicketingBusiness.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepository)
        {
            _userRepo = userRepository;
        }

        public async Task<List<SupportUserDto>> GetSupportUsersAsync()
        {
            var users = await _userRepo.GetSupportUsersAsync();

            return users.Select(u => new SupportUserDto
            {
                Id = u.Id,
                FullName = u.FullName
            }).ToList();
        }
        public async Task<List<SupportUserDto>> GetSupportUsersAsyncToDashboard()
        {
            var users = await _userRepo.GetSupportUsersAsyncToDashboard();
            return users.Where(u => u.SupportTickets.Count != 0)
            .Select(u => new SupportUserDto
            {
                Id = u.Id,
                FullName = u.FullName
            }).ToList();
        }
        public async Task<(List<UserDto>, int TotalCount)> GetUsersByRoleAsync(int pageNumber ,
            int pageSize , UserRole? Role, string? Search)
        {
            var (users , Total ) = await _userRepo.GetUsersWithFiltersAsync(pageNumber ,
               pageSize , Role , Search);

            var returnUsers = users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Mobile = u.Mobile,
                Email = u.Email,
                Role = u.Role,
                TicketCount = u.Role ==UserRole.Client ? u.ClientTickets.Count : u.SupportTickets.Count ,
                IsActive = u.IsActive
            }).ToList();
            return (returnUsers, Total);
        }

        public async Task<ServiceResponse<string>> ActivateUserAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                return ServiceResponse<string>.FailureResponse("User not found");
            if ( user.IsActive == true)
                return ServiceResponse<string>.FailureResponse("User is already active.");

            user.IsActive = true;
            await _userRepo.Update(user);
            return ServiceResponse<string>.SuccessResponse("User activated successfully.");

        }

        public async Task<ServiceResponse<string>> DeactivateUserAsync(int userId)
        {
            var user = await _userRepo.GetByIdWithTicketsAsync(userId);
            if (user == null)
                return ServiceResponse<string>.FailureResponse("User not found");
            if (user.IsActive == false)
                return ServiceResponse<string>.FailureResponse("User is already deactivated.");

            bool HasOpenTickets = user.SupportTickets.Any(t => t.Status != TicketStatus.Closed)
                || user.ClientTickets.Any(t => t.Status != TicketStatus.Closed);
            if (HasOpenTickets)
                return ServiceResponse<string>.FailureResponse("Cannot deactivate user with open or assigned tickets.");
           
            user.IsActive = false;
            await _userRepo.Update(user);
            return ServiceResponse<string>.SuccessResponse("User deactivated successfully.");

        }
        public async Task<SupportStatsDto> GetSupportStatisticsAsync()
        {
            var (total, active) = await _userRepo.GetSupportCountsAsync();
            return new SupportStatsDto
            {                
                ActiveSupport = active,
                InactiveSupport = total - active
            };
        }
    }
}
