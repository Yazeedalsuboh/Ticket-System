using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupportTicketingBusiness.DTO;
using SupportTicketingBusiness.Interface;
using SupportTicketingData.Entities;
using SupportTicketingData.Enums;
using static OpenQA.Selenium.PrintOptions;

namespace SupportTicketingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet("support-only")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetSupportUsers()
        {
            var users = await _userService.GetSupportUsersAsync();
            return Ok(new { data = users, message = "Users received successfully" });
        }

        [HttpGet("support")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetSupportUsersAsyncToDashboard()
        {
            var users = await _userService.GetSupportUsersAsyncToDashboard();
            return Ok(new { data = users, message = "Users received successfully" });
        }

        [HttpPost("users-by-role")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetUsersByRole([FromBody]GetUsersByRoleDto dto)
        {
            var (result, TotalCount) = await _userService.GetUsersByRoleAsync( dto.pageNumber, dto.pageSize, dto.Role,  dto.Search);
            return Ok(new { data = new {
                                    result,
                                    totalCount = TotalCount,
                                    PageNumber = dto.pageNumber,
                                    PageSize = dto.pageSize
                                    }
                , message = "Users received successfully" 
            });
        }

        [HttpPut("activate/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var response = await _userService.ActivateUserAsync(id);

            if (response.Success)
            {
                return Ok(new { message = response.Data });
            }
            else
            {
                return BadRequest(new { message = response.Message });
            }
        }

        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var response = await _userService.DeactivateUserAsync(id);

            if (response.Success)
            {
                return Ok(new { message = response.Data });
            }
            else
            {
                return BadRequest(new { message = response.Message });
            }
        }
    }
}
