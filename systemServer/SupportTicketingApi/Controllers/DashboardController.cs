using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupportTicketingBusiness.Interface;
using SupportTicketingBusiness.Services;

namespace SupportTicketingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;

        public DashboardController(IUserService userService, ITicketService ticketService)
        {
            _userService = userService;
            _ticketService = ticketService;
        }

        [HttpGet("support-stats")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetSupportStats()
        {
            var stats = await _userService.GetSupportStatisticsAsync();
            return Ok(new { data = stats, message = "success"});
        }

        [HttpGet("ticket-stats")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetTicketStats()
        {
            var stats = await _ticketService.GetTicketStatsAsync();
            return Ok(new { data = stats, message = "success" });
        }

        [HttpGet("support-ticket-stats/{supportId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetSupportTicketStats(int supportId)
        {
            var stats = await _ticketService.GetSupportTicketStatsAsync(supportId);
            return Ok(new { data = stats, message = "success" });
        }

        [HttpGet("product-ticket-stats/{productId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetProductTicketStats(int productId)
        {
            var stats = await _ticketService.GetProductTicketStatsAsync(productId);
            return Ok(new { data = stats, message = "success" });
        }

        [HttpGet("Summary")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Summary()
        {
            var Summary = await _ticketService.GetSummaryAsync();
            return Ok(new { data = Summary, message = "success" });
        }
    }
}
