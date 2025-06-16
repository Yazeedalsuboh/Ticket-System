using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupportTicketingBusiness.Interface;
using SupportTicketingBusiness.DTO;
using Microsoft.AspNetCore.Authorization;
using SupportTicketingBusiness.Services;

namespace SupportTicketingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IAuthService AuthService, ITokenService tokenService)
        {
            _authService = AuthService;
            _tokenService = tokenService;
        }

        [HttpPost("client-register")]
        public async Task<IActionResult> Register(RegisterUser registerDto)
        {
            var response = await _authService.RegisterClientAsync(registerDto);

            if (response.Success)
            {
                return Ok(new { message = response.Message });
            }

            return BadRequest(new { message = response.Message });
        }

        [HttpPost("support-register")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RegisterSupport(RegisterUser registerDto)
        {
            var response = await _authService.RegisterSupportAsync(registerDto);

            if (response.Success)
            {
                return Ok(new { message = response.Message });
            }

            return BadRequest(new { message = response.Message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUser loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);
            if (response.Success)
            {
                return Ok(new { data = response.Data, message = response.Message });
            }
            if (response.Message.Contains("Invalid Username or Password"))
            {
                return Unauthorized(new { message = response.Message });
            }        
            return BadRequest(new { message = response.Message });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                var tokenResponse = await _tokenService.RefreshAccessToken(refreshTokenRequest.RefreshToken);
                return Ok(new { data = tokenResponse, message = "success" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("change-password")]
        [Authorize(Roles = "Client , Support")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var response = await _authService.ChangePasswordAsync(dto);

            if (response.Success)
            {
                return Ok(new { message = response.Message });
            }

            if (response.Message.Contains("not found") || response.Message.Contains("not active"))
            {
                return Unauthorized(new { message = response.Message });
            }

            return BadRequest(new { message = response.Message });
        }

        [HttpPost("Send-verification-code")]
        public async Task<IActionResult> SendVerificationCode([FromBody] ForgotPasswordDto dto)
        {
            var response = await _authService.SendVerificationCode(dto.Email);

            if (response.Success)
            {
                return Ok(new { message = response.Message });
            }

            if (response.Message.Contains("not found"))
            {
                return NotFound(new { message = response.Message });
            }

            return BadRequest(new { message = response.Message });
        }
        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] ForgotPasswordDto dto)
        {
            var response = await _authService.Verify(dto.Email, dto.Code);

            if (response.Success)
            {
                return Ok(new { message = response.Message });
            }

            return BadRequest(new { message = response.Message });
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {           
           var response = await _authService.ForgotPasswordAsync(dto.Email, dto.Password);

            if (response.Success)
            {
                return Ok(new { message = response.Message });
            }

            if (response.Message.Contains("User not found"))
            {
                return NotFound(new { message = response.Message });
            }

            return BadRequest(new { message = response.Message });
        }
    }
}
