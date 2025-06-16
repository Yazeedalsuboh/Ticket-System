using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupportTicketingBusiness.DTO;
using SupportTicketingBusiness.Interface;
using SupportTicketingData.Enums;

namespace SupportTicketingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("add-ticket")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> AddTicket([FromForm] AddTicketDto dto)
        {
            var response = await _ticketService.AddTicketAsync(dto);

            if (response.Success)
                return Ok(new { message = response.Message });

            return BadRequest(new { message = response.Message });
        }
        
        [HttpPost("filter-ticket-list")]
        [Authorize(Roles = "Client , Support , Manager")]
        public async Task<IActionResult> GetAllTicketsWithFilter([FromBody] TicketFilterDto filter)
        {
            var response = await _ticketService.GetAllTicketsWithFilterAsync(filter);

            if (response.Success)
            {
                var (tickets, totalCount) = response.Data;
                return Ok(new
                {
                    data = new
                    {
                        tickets,
                        totalCount,
                        pageNumber = filter.PageNumber,
                        pageSize = filter.PageSize
                    },
                    message = response.Message
                });
            }
            else
            {
                return BadRequest(new { message = response.Message });
            }
        }


        [HttpGet("{Id}")]
        [Authorize(Roles = "Client , Support , Manager")]
        public async Task<IActionResult> GetTicketsbyId(int Id)
        {
            var response = await _ticketService.GetTicketByIdAsync(Id);

            if (response.Success)
            {
                return Ok(new { data = response.Data, message = response.Message });
            }
            else
            {
                if (response.Message.Contains("Access denied", StringComparison.OrdinalIgnoreCase))
                    return Unauthorized(new { message = response.Message });

                return NotFound(new { message = response.Message });
            }
        }

        [HttpPost("attachments")]
        [Authorize(Roles = "Client , Support")]
        public async Task<IActionResult> AddAttachments( [FromForm] AddAttachmentsDto dto)
        {
            var response = await _ticketService.AddAttachmentsToTicketAsync(dto);

            if (response.Success)
            {
                return Ok(new { message = response.Message });
            }
            else
            {
                return BadRequest(new { message = response.Message });
            }

        }

        [HttpPut("update-ticket")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> UpdateTicket([FromForm] UpdateTicketDto dto)
        {
            var response = await _ticketService.UpdateTicket(dto);

            if (response.Success)
            {
                return Ok(new { message = response.Message });
            }
            else
            {
                return BadRequest(new { message = response.Message });
            }
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AssignTicket([FromBody] AssignTicketDto dto)
        {
            var response = await _ticketService.AssignTicketAsync(dto);

            if (response.Success)
            {
                return Ok(new { message = response.Message });
            }
            else
            {
                return BadRequest(new { message = response.Message });
            }
        }

        [HttpPut("close-ticket/{Id}")]
        [Authorize(Roles = "Support")]
        public async Task<IActionResult> CloseTicket(int Id)
        {
            var response = await _ticketService.CloseTheTicket(Id);

            if (response.Success)
            {
                return Ok(new { message = response.Message });
            }
            else
            {
                return BadRequest(new { message = response.Message });
            }
        }
    }
}
