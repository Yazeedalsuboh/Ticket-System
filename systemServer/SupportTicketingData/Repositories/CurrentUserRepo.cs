using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SupportTicketingData.Entities;
using SupportTicketingData.Interface;

namespace SupportTicketingData.Repositories
{
    public class CurrentUserRepo : ICurrentUserRepo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserRepo(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        public string Email
        {
            get
            {
                var emailClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email);
                return emailClaim?.Value ?? throw new UnauthorizedAccessException("User email not found.");
            }
        }

        public string Role
        {
            get
            {
                var RoleClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role);
                return RoleClaim?.Value ?? throw new UnauthorizedAccessException("User role not found.");
            }
        }
    }
}
