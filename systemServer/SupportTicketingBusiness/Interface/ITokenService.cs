using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportTicketingData.Entities;
using SupportTicketingBusiness.DTO;

namespace SupportTicketingBusiness.Interface
{
    public interface ITokenService
    {
        string GenerateToken(Users User);
        Task<string> GenerateAndSaveRefreshToken(Users User);
        Task<TokenResponseDTO> RefreshAccessToken(string refreshToken);
    }
}
