using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportTicketingBusiness.DTO;
using SupportTicketingBusiness.Services;

namespace SupportTicketingBusiness.Interface
{
    public interface IAuthService
    {
        Task<ServiceResponse<bool>> RegisterSupportAsync(RegisterUser UserDto);
        Task<ServiceResponse<bool>> RegisterClientAsync(RegisterUser UserDto);
        Task<ServiceResponse<TokenResponseDTO>> LoginAsync(LoginUser UserDto);
        Task<ServiceResponse<bool>> ChangePasswordAsync(ChangePasswordDto Dto);
        Task<ServiceResponse<bool>> SendVerificationCode(string email);
        Task<ServiceResponse<bool>> Verify(string email, string code);
        Task<ServiceResponse<bool>> ForgotPasswordAsync(string email, string newPassword);
    }
}
