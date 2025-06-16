using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NPOI.SS.Formula.Functions;
using SupportTicketingBusiness.DTO;
using SupportTicketingBusiness.Interface;
using SupportTicketingData.Entities;
using SupportTicketingData.Interface;

namespace SupportTicketingBusiness.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepo _UserRepository;

        public TokenService(IConfiguration config, IUserRepo UserRepository)
        {
            _config = config;
            _UserRepository = UserRepository;
        }

        public string GenerateToken(Users User)
        {          
            //store User information inside the Token in claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, User.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, User.FullName),
                new Claim(ClaimTypes.Role,User.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, User.Email)

            };

            //Token protection usig key and HMAC
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Generate Token
            var token = new JwtSecurityToken
            (
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
           );
            //return the Token as string 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var RandomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(RandomNumber);
            }
            return Convert.ToBase64String(RandomNumber);
        }

        public async Task<string> GenerateAndSaveRefreshToken(Users User)
        {
            var RefreshToken = GenerateRefreshToken();
            User.RefreshToken = RefreshToken;
            User.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(3);

            await _UserRepository.Update(User);
           
            return RefreshToken;
        }

        public async Task<TokenResponseDTO> RefreshAccessToken(string refreshToken)
        {
            var player = (await _UserRepository.GetAsync(p => p.RefreshToken == refreshToken)).FirstOrDefault();
            if (player == null || player.RefreshTokenExpiryTime < DateTime.UtcNow)
            {   
                throw new UnauthorizedAccessException("Invalid or expired refresh token");
            }
            return new TokenResponseDTO
            {
                Token = GenerateToken(player),
                RefreshToken = await GenerateAndSaveRefreshToken(player)
            };
        }

    }
}
