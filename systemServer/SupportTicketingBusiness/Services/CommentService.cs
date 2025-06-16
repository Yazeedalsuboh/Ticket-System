using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OpenQA.Selenium;
using SupportTicketingBusiness.DTO;
using SupportTicketingBusiness.Interface;
using SupportTicketingData.Entities;
using SupportTicketingData.Enums;
using SupportTicketingData.Interface;

namespace SupportTicketingBusiness.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICurrentUserRepo _currentUser;
        private readonly ITicketRepo _ticketRepo;
        private readonly ICommentRepo _commentRepo;
        private readonly INotificationService _notificationService;
        public CommentService(ICurrentUserRepo currentUser , ITicketRepo ticketRepo, ICommentRepo commentRepo ,INotificationService notificationService)
        {
            _currentUser = currentUser;
            _ticketRepo = ticketRepo;
            _commentRepo = commentRepo;
            _notificationService = notificationService;
        }

        public async Task<ServiceResponse<bool>> AddCommentAsync(AddCommentDto dto)
        {
            var UserId = _currentUser.GetUserId();
            var userRole = _currentUser.Role;

            var ticket = await _ticketRepo.GetByIdAsync(dto.Id);
            if (ticket == null)
                return ServiceResponse<bool>.FailureResponse("Ticket not found.");
            if (string.IsNullOrWhiteSpace(dto.Message) )
                return ServiceResponse<bool>.FailureResponse("Message cannot be empty.");
            var comment = new Comment
            {
                Message = dto.Message,
                TicketId = dto.Id,
                UserId = UserId
            };
           
            await _commentRepo.AddAsync(comment);

            string targetUserId = null;
            if (userRole == "Client" && ticket.SupportId != null)
                targetUserId = ticket.SupportId.ToString();
            else if (userRole == "Support")
                targetUserId = ticket.ClientId.ToString();
            if (targetUserId != null)
            {
                await _notificationService.CreateAndSendNotificationAsync(targetUserId,
                      $"{userRole} Add comment",
                      $"/tickets/{dto.Id}"
                );
            }
            return ServiceResponse<bool>.SuccessResponse(true, "Comment added successfully.");
        }

        public async Task<ServiceResponse<List<CommentDto>>> GetCommentsByTicketIdAsync(int ticketId)
        {
            var ticket = await _ticketRepo.GetByIdAsync(ticketId);
            if (ticket == null)
            {
                return ServiceResponse<List<CommentDto>>.FailureResponse("Ticket not found.");
            }

            var comments = await _commentRepo.GetCommentsByTicketIdAsync(ticketId);
            var commentDtos = comments.Select(c => new CommentDto
            {
                Message = c.Message,
                Username = c.User.FullName,
                CreatedAt = c.CreatedAt
            }).ToList();

            return ServiceResponse<List<CommentDto>>.SuccessResponse(commentDtos, "Comments retrieved successfully.");

        }
    }
}
