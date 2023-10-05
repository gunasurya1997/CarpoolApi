using CarpoolService.Contracts.Interfaces.ServiceInterface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DTO = CarpoolService.Contracts.DTOs;

namespace CarpoolService.BLL.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(string issuer, string audience, string key, DTO.User authenticatedUser)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
            {
                new Claim("UserId", authenticatedUser.Id.ToString()),
                new Claim(ClaimTypes.Email, authenticatedUser.Email),
            };

            JwtSecurityToken token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
