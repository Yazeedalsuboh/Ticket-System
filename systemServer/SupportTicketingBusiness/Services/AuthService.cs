using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NPOI.SS.Formula.Functions;
using SupportTicketingBusiness.DTO;
using SupportTicketingBusiness.Interface;
using SupportTicketingData.Entities;
using SupportTicketingData.Interface;
using SupportTicketingData.Enums;
using Serilog;
using Log = Serilog.Log;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using NPOI.SS.Formula.Eval;
using OpenQA.Selenium;
namespace SupportTicketingBusiness.Services
{
    public class AuthService :IAuthService
    {
        private IUserRepo _userRepo;
        private readonly ITokenService _tokenService;
        private readonly ICurrentUserRepo _currentUserService;
        private readonly IEmailService _emailService;
        private readonly IVerificationCodeRepo _verificationCodeRepo;

        public AuthService(IUserRepo userRepo, ITokenService tokenService , ICurrentUserRepo currentUserService, IEmailService emailService,IVerificationCodeRepo verificationCodeRepo)
        {
            _currentUserService = currentUserService;
            _userRepo = userRepo;
            _tokenService = tokenService;
            _emailService = emailService;
            _verificationCodeRepo = verificationCodeRepo;
        }

        private bool IsStrongPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        private async Task<ServiceResponse<Users>> CheckAndCreat(RegisterUser UserDto)
        {
            var ExistingUsers = await _userRepo.GetAsync(u => u.Email == UserDto.Email
                                             && u.FullName == UserDto.FullName);
            var ExistingUser = ExistingUsers.FirstOrDefault();
            if (ExistingUser != null)
            {
                return ServiceResponse<Users>.FailureResponse("User with this Username already exists.");
            }
            if (!IsStrongPassword(UserDto.Password))
            {
                return ServiceResponse<Users>.FailureResponse("Password must be at least 8 characters long, contain an uppercase letter, a lowercase letter, a number, and a special character.");
            }

            var user = new Users
            {
                FullName = UserDto.FullName,
                Image = UserDto.Image,
                PasswordHash = new PasswordHasher<Users>().HashPassword(null, UserDto.Password),
                Email = UserDto.Email,
                Mobile = UserDto.Mobile,
                Address = UserDto.Address,
                DateOfBirth = UserDto.DateOfBirth,               
            };
            return ServiceResponse<Users>.SuccessResponse(user);
        }

        private string GenerateStrongPassword(int length = 12)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            var res = new StringBuilder();
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (res.Length < length)
                {
                    rng.GetBytes(uintBuffer);
                    var num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return res.ToString();
        }
        public async Task<ServiceResponse<bool>> RegisterClientAsync(RegisterUser UserDto)
        {
            var checkResult = await CheckAndCreat(UserDto);
            if (!checkResult.Success)
            {
                return ServiceResponse<bool>.FailureResponse(checkResult.Message);
            }
            var User = checkResult.Data;
            User.Role = UserRole.Client;
            await _userRepo.AddAsync(User);
            return ServiceResponse<bool>.SuccessResponse(true, "Client registration successful.");
        }

        public async Task<ServiceResponse<bool>> RegisterSupportAsync(RegisterUser UserDto)
        {            
            UserDto.Password = GenerateStrongPassword();
            var checkResult = await CheckAndCreat(UserDto);
            if (!checkResult.Success)
            {
                return ServiceResponse<bool>.FailureResponse(checkResult.Message);
            }
            var User = checkResult.Data;
            User.Role = UserRole.Support;
            await _userRepo.AddAsync(User);
            _emailService.SendEmailAsync(User.Email, "Support Account created",
                $"Your account has been created successfully.<br/>Your password is : " +
                $"<strong>{UserDto.Password}</strong> ");

            return ServiceResponse<bool>.SuccessResponse(true, "Client registration successful.");

        }

        public async Task<ServiceResponse<bool>> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var userId = _currentUserService.GetUserId();
            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null || !user.IsActive)
                return ServiceResponse<bool>.FailureResponse("User not found or not active.");

            var passwordHasher = new PasswordHasher<Users>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.CurrentPassword);

            if (result == PasswordVerificationResult.Failed)
                return ServiceResponse<bool>.FailureResponse("Current password is incorrect.");

            if (!IsStrongPassword(dto.NewPassword))
                return ServiceResponse<bool>.FailureResponse("New password is not strong enough.");

            user.PasswordHash = passwordHasher.HashPassword(user, dto.NewPassword);
            await _userRepo.Update(user);
            return ServiceResponse<bool>.SuccessResponse(true, "Password changed successfully.");
        }

        public async Task<ServiceResponse<bool>> SendVerificationCode(string email)
        {
            var user = await _userRepo.GetAsync(u => u.Email == email);
            var existingUser = user.FirstOrDefault();

            if (existingUser == null)
                return ServiceResponse<bool>.FailureResponse("User not found with this email.");

            var code = new Random().Next(100000, 999999).ToString();

            var resetCode = new VerificationCode
            {
                Email = email,
                Code = code,
                ExpiryTime = DateTime.UtcNow.AddMinutes(5)
            };

            await _verificationCodeRepo.AddAsync(resetCode);
            await _emailService.SendEmailAsync(email, "Password Reset Code", $"Your verification code is: {code}");
            return ServiceResponse<bool>.SuccessResponse(true, "A verification code has been sent to your email.");
        }
        public async Task<ServiceResponse<bool>> Verify(string email, string code)
        {
            var isVerified = await _verificationCodeRepo.Verify(email, code);

            if (!isVerified)
            {
                return ServiceResponse<bool>.FailureResponse("Verification failed.");
            }

            return ServiceResponse<bool>.SuccessResponse(true, "Code verified successfully.");
        }
        public async Task<ServiceResponse<bool>> ForgotPasswordAsync(string email, string newPassword)
        {
            var user = await _userRepo.GetAsync(u => u.Email == email);
            var found = user.FirstOrDefault();
            if (found == null)
                return ServiceResponse<bool>.FailureResponse("User not found.");
            if (!IsStrongPassword(newPassword))
                return ServiceResponse<bool>.FailureResponse("New password is not strong enough.");

            found.PasswordHash = new PasswordHasher<Users>().HashPassword(null, newPassword);
            await _userRepo.Update(found);
            return ServiceResponse<bool>.SuccessResponse(true, "Password reset successfully.");
        }
        public async Task<ServiceResponse<TokenResponseDTO>> LoginAsync(LoginUser UserDto)
        {
            Log.Information("Login attempt: Identifier={Identifier}", UserDto.Identifier);
            var users = await _userRepo.GetAsync(u =>
                                                     u.FullName == UserDto.Identifier ||
                                                     u.Email == UserDto.Identifier ||
                                                     u.Mobile == UserDto.Identifier
            );
            var user = users.FirstOrDefault();
            if (users == null || new PasswordHasher<Users>().VerifyHashedPassword(user, user.PasswordHash, UserDto.Password) != PasswordVerificationResult.Success)
            {
                Log.Warning("Login failed: User not found for Identifier={Identifier} or Invalid Password", UserDto.Identifier);
                return ServiceResponse<TokenResponseDTO>.FailureResponse("Invalid Username or Password");
            }
            if (!user.IsActive)
            {
                Log.Warning("Your account is deactivated.");
                return ServiceResponse<TokenResponseDTO>.FailureResponse("Your account is deactivated.");
            }
            Log.Information("Login success: UserId={UserId}", user.Id);
            var tokenResponse = new TokenResponseDTO
            {
                Token = _tokenService.GenerateToken(user),
                RefreshToken = await _tokenService.GenerateAndSaveRefreshToken(user)
            };

            return ServiceResponse<TokenResponseDTO>.SuccessResponse(tokenResponse, "Login successful.");
        }
    }
}
