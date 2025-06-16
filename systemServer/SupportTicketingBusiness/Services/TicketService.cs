using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OpenQA.Selenium;
using SupportTicketingBusiness.DTO;
using SupportTicketingBusiness.Interface;
using SupportTicketingData.Entities;
using SupportTicketingData.Enums;
using SupportTicketingData.Interface;
using SupportTicketingData.Repositories;

namespace SupportTicketingBusiness.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepo _ticketRepo;
        private readonly IUserRepo _userRepo;
        private readonly IProductRepo _productRepo;
        private readonly IGenericRepo<Attachment> _attachmentRepo;
        private readonly IGenericRepo<Comment> _commentRepo;
        private readonly ICurrentUserRepo _currentUser;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TicketService(
            ITicketRepo ticketRepo,
            IUserRepo userRepo,
            IProductRepo productRepo,
            IGenericRepo<Attachment> attachmentRepo,
            IGenericRepo<Comment> commentRepo,
            ICurrentUserRepo currentUser,
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor)
        {
            _attachmentRepo = attachmentRepo;
            _userRepo = userRepo;
            _ticketRepo = ticketRepo;
            _productRepo = productRepo;
            _commentRepo = commentRepo;
            _currentUser = currentUser;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<string>> AddTicketAsync(AddTicketDto dto)
        {
            var products = await _productRepo
                .GetAsync(p => p.Name.ToLower() == dto.ProductName.ToLower());
            var product = products.FirstOrDefault();
            if (product == null)
                return ServiceResponse<string>.FailureResponse("Product not found.");
            var UserId = _currentUser.GetUserId();
            var ticket = new Ticket
            {
                Title = dto.Title,
                Description = dto.Description,
                ProductId = product.Id,
                ClientId = UserId,
                Status = TicketStatus.Open,
                Attachments = new List<Attachment>()
            };

            if (dto.Attachments != null && dto.Attachments.Any())
            {
                string folderPath = Path.Combine(_env.WebRootPath, "attachments");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                foreach (var file in dto.Attachments)
                {
                    var allowedExtensions = new[] { ".jpg", ".png" };
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                        return ServiceResponse<string>.FailureResponse("Invalid file type. Allowed: .jpg, .png");

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    ticket.Attachments.Add(new Attachment
                    {
                        FilePath = $"attachments/{fileName}"
                    });
                }
            }

            await _ticketRepo.AddAsync(ticket);
            return ServiceResponse<string>.SuccessResponse("Ticket created successfully.");

        }

        public async Task<List<AllTiketDto>> GetAllTicketsAsync()
        {
            var tickets = await _ticketRepo.GetAllWithDetailsAsync();

            var result = tickets.Select(t => new AllTiketDto
            {
                Id = t.Id,
                Title = t.Title,
                Status = t.Status,
                ClientName = t.Client?.FullName ?? string.Empty,
                SupportName = t.Support?.FullName ?? string.Empty,
                ProductName = t.Product?.Name ?? string.Empty
            }).ToList();

            return result;
        }

        public async Task<ServiceResponse<(List<AllTiketDto> Tickets, int TotalCount)>> GetAllTicketsWithFilterAsync(TicketFilterDto filter)
        {
            var (tickets , total ) = await _ticketRepo.GetAllWithFiltersAsync(
                                            filter.PageNumber,
                                            filter.PageSize,
                                            filter.Status,
                                            filter.UserRoleFilter,
                                            filter.SearchTitle,
                                            filter.ProductId,
                                            filter.UserIdFilter,
                                            filter.SortByCreatedAtAsc
            );
            
            var result = tickets.Select(t => new AllTiketDto
            {
                Id = t.Id,
                Title = t.Title,
                Priority= t.Priority,
                Status = t.Status,
                ClientName = t.Client?.FullName ?? string.Empty,
                SupportName = t.Support?.FullName ?? string.Empty,
                ProductName = t.Product?.Name ?? string.Empty,
                CreatedAt = t.CreatedAt
            }).ToList();
            return ServiceResponse<(List<AllTiketDto>, int)>.SuccessResponse((result, total), "Tickets retrieved successfully.");
        }
        public async Task<ServiceResponse<TicketDetailsDto>> GetTicketByIdAsync(int Id)
        {
            var ticket = await _ticketRepo.GetByIdWithAllDetailsAsync(Id);
            var CurrentUser = _currentUser.GetUserId();
            var RoleUser = _currentUser.Role;
            if (ticket == null)
            {
                return ServiceResponse<TicketDetailsDto>.FailureResponse($"Ticket with ID {Id} was not found.");
            }
            var SupportStatus = true;
            if (ticket.Support != null)
                SupportStatus = ticket.Support.IsActive;

            if (ticket.ClientId == CurrentUser || ticket.SupportId == CurrentUser || RoleUser == "Manager")
            {
                var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}:" +
                    $"//{_httpContextAccessor.HttpContext.Request.Host}/";

                var dto = new TicketDetailsDto
                {
                    Title = ticket.Title,
                    Description = ticket.Description,                    
                    Status = Enum.GetName(typeof(TicketStatus),ticket.Status),
                    ClientName = ticket.Client.FullName,
                    SupportName = SupportStatus ? (ticket.Support?.FullName ?? string.Empty) : (ticket.Support?.FullName + " (Not Active)" ?? string.Empty),
                    ProductName = ticket.Product.Name,
                    Priority = ticket.Priority,
                    Attachments = ticket.Attachments.Select(a => new AttachmentDto
                    {
                        FileName = Path.GetFileName(a.FilePath),
                        FileUrl = baseUrl + a.FilePath
                    }).ToList(),

                    Comments = ticket.Comments
                              .OrderBy(c => c.CreatedAt)
                              .Select(c => new CommentDto
                              {
                                  Message = c.Message,
                                  CreatedAt = c.CreatedAt,
                                  Username = c.User.FullName
                              }).ToList()
                };

                return ServiceResponse<TicketDetailsDto>.SuccessResponse(dto, "Ticket retrieved successfully.");
            }
            else
                return ServiceResponse<TicketDetailsDto>.FailureResponse("Invalid operation: Access denied.");
        }

        public async Task<ServiceResponse<string>> AddAttachmentsToTicketAsync(AddAttachmentsDto dto)
        {
            var ticket = await _ticketRepo.GetTicketWithAttachmentsAsync(dto.Id);

            if (ticket == null)
                return ServiceResponse<string>.FailureResponse("Ticket not found.");

            string folderPath = Path.Combine(_env.WebRootPath, "attachments");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var attachments = new List<Attachment>();

            foreach (var file in dto.Files)
            {

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                catch (Exception ex)
                {
                    return ServiceResponse<string>.FailureResponse($"Error saving file: {file.FileName}. Details: {ex.Message}");
                }

                attachments.Add(new Attachment
                {
                    FilePath = $"attachments/{fileName}",
                    TicketId = dto.Id
                });

            }
            await _attachmentRepo.AddRangeAsync(attachments);
            await _ticketRepo.Update(ticket);
            return ServiceResponse<string>.SuccessResponse("Attachments added successfully.");

        }

        public async Task<ServiceResponse<string>> UpdateTicket(UpdateTicketDto dto)
        {
            var ticket = await _ticketRepo.GetByIdAsync(dto.Id);
            if (ticket == null)
                return ServiceResponse<string>.FailureResponse("Ticket not found.");
            if (ticket.Status != TicketStatus.Assign)
            {
                var products = await _productRepo
                    .GetAsync(p => p.Name.ToLower() == dto.ProductName.ToLower());
                var product = products.FirstOrDefault();
                if (product == null)
                    return ServiceResponse<string>.FailureResponse("Product not found.");

                ticket.Title = dto.Title;
                ticket.Priority = dto.Priority;
                ticket.Description = dto.Description;
                ticket.ProductId = product.Id;

                var oldAttachments = await _attachmentRepo.GetAsync(a => a.TicketId == dto.Id);
                var oldAttachmentFileNames = oldAttachments.Select(a => Path.GetFileName(a.FilePath)).ToList();
                var newFileNames = dto.Attachments?.Select(f => f.FileName).ToList() ?? new List<string>();

                var attachmentsToDelete = oldAttachments
                    .Where(a => !newFileNames.Contains(Path.GetFileName(a.FilePath)))
                    .ToList();
                foreach (var attachment in attachmentsToDelete)
                {
                    var fullPath = Path.Combine(_env.WebRootPath, attachment.FilePath);
                    try
                    {
                        if (File.Exists(fullPath))
                            File.Delete(fullPath);

                        await _attachmentRepo.Delete(attachment);
                    }
                    catch (Exception ex)
                    {
                        return ServiceResponse<string>.FailureResponse($"Error deleting file {attachment.FilePath}: {ex.Message}");
                    }
                }

                var filesToAdd = dto.Attachments?
                    .Where(f => !oldAttachmentFileNames.Contains(f.FileName))
                    .ToList();

                if (filesToAdd != null && filesToAdd.Any())
                {
                    string folderPath = Path.Combine(_env.WebRootPath, "attachments");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var attachments = new List<Attachment>();

                    foreach (var file in filesToAdd)
                    {
                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                        var filePath = Path.Combine(folderPath, fileName);
                        try
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            attachments.Add(new Attachment
                            {
                                FilePath = $"attachments/{fileName}",
                                TicketId = dto.Id
                            });
                        }
                        catch (Exception ex)
                        {
                            return ServiceResponse<string>.FailureResponse($"Error saving file {file.FileName}: {ex.Message}");
                        }
                    }

                    await _attachmentRepo.AddRangeAsync(attachments);
                }

                await _ticketRepo.Update(ticket);
            }
            return ServiceResponse<string>.SuccessResponse("Ticket updated successfully.");
        }
        public async Task<ServiceResponse<string>> CloseTheTicket(int TicketId)
        {

            var ticket = await _ticketRepo.GetByIdAsync(TicketId);
            if (ticket == null)
                return ServiceResponse<string>.FailureResponse("Ticket not found.");

            if (ticket.Status == TicketStatus.Closed)
                return ServiceResponse<string>.FailureResponse("Ticket is already closed.");

            ticket.Status = TicketStatus.Closed;
            await _ticketRepo.Update(ticket);
            return ServiceResponse<string>.SuccessResponse("Ticket closed successfully.");

        }
        public async Task<ServiceResponse<string>> AssignTicketAsync(AssignTicketDto Dto)
        {
            try
            {
                var ticket = await _ticketRepo.GetByIdAsync(Dto.TicketId);
                if (ticket == null)
                    return ServiceResponse<string>.FailureResponse("Ticket not found.");

                if (ticket.Status == TicketStatus.Closed)
                    return ServiceResponse<string>.FailureResponse("Ticket is closed.");

                var user = await _userRepo.GetByIdAsync(Dto.EmployeeId);

                if (user == null || user.Role != UserRole.Support)
                    return ServiceResponse<string>.FailureResponse("User is not a valid support employee.");

                ticket.SupportId = Dto.EmployeeId;
                ticket.Status = TicketStatus.Assign;

                await _ticketRepo.Update(ticket);
                return ServiceResponse<string>.SuccessResponse("Ticket assigned successfully.");

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error assigning ticket: {ex.Message}");
            }
        }

        public async Task<TicketStatsDto> GetTicketStatsAsync()
        {
            var tickets = await _ticketRepo.GetAllAsync();

            var stats = new TicketStatsDto
            {              
                Opened = tickets.Count(t => t.Status == TicketStatus.Open),
                Assigned = tickets.Count(t => t.Status == TicketStatus.Assign),
                Closed = tickets.Count(t => t.Status == TicketStatus.Closed)
            };

            return stats;
        }
        public async Task<SupportTicketStatsDto> GetSupportTicketStatsAsync(int supportId)
        {
            var tickets = await _ticketRepo.GetTicketsBySupportIdAsync(supportId);

            return new SupportTicketStatsDto
            {
                Closed = tickets.Count(t => t.Status == TicketStatus.Closed),
                NotClosed = tickets.Count(t => t.Status != TicketStatus.Closed)
            };
        }
        public async Task<ProductTicketStatsDto> GetProductTicketStatsAsync(int productId)
        {
            var tickets = await _ticketRepo.GetTicketsByProductIdAsync(productId);

            return new ProductTicketStatsDto
            {
                Closed = tickets.Count(t => t.Status == TicketStatus.Closed),
                NotClosed = tickets.Count(t => t.Status != TicketStatus.Closed)
            };
        }

        public async Task<List<SummaryDto>> GetSummaryAsync()
        {
            var totalTickets = await _ticketRepo.GetTotalCountAsync();
            var totalSupport = await _userRepo.GetSupportsUsersAsync();
            var totalClient = await _userRepo.GetClientsUsersAsync();
            var totalProducts = await _productRepo.GetTotalCountAsync();

            return new List<SummaryDto>
            {
                new SummaryDto { Title = "Total Tickets", Amount = totalTickets },
                new SummaryDto { Title = "Total Support ", Amount = totalSupport },
                new SummaryDto { Title = "Total Client ", Amount = totalClient },
                new SummaryDto { Title = "Total Products", Amount = totalProducts }
            };
        }
    }
}
