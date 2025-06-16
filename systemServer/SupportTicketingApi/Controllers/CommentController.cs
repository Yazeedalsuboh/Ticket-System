using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupportTicketingBusiness.DTO;
using SupportTicketingBusiness.Interface;
using SupportTicketingBusiness.Services;

namespace SupportTicketingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("add-comment")]
        [Authorize(Roles = "Client , Support")]
        public async Task<IActionResult> AddComment(AddCommentDto dto)
        {
            var response = await _commentService.AddCommentAsync(dto);

            if (response.Success)
            {
                return Ok(new { message = response.Message });
            }

            if (response.Message.Contains("Ticket not found"))
            {
                return NotFound(new { message = response.Message });
            }

            return BadRequest(new { message = response.Message });
        }

        [HttpGet("comments/{ticketId}")]
        [Authorize(Roles = "Client , Support ,Manager")]
        public async Task<IActionResult> GetCommentsByTicketId(int ticketId)
        {
            var response = await _commentService.GetCommentsByTicketIdAsync(ticketId);

            if (response.Success)
            {
                return Ok(new { data = response.Data, message = response.Message });
            }

            if (response.Message.Contains("Ticket not found"))
            {
                return NotFound(new { message = response.Message });
            }

            return BadRequest(new { message = response.Message });
        }
    }
}
