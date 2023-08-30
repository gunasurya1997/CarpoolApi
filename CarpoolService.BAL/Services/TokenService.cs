﻿using CarpoolService.Contracts;
using CarPoolService.Models.Interfaces.Service_Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarpoolService.BAL.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(string issuer, string audience, string key, UserDto authenticatedUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("UserId", authenticatedUser.UserId.ToString()), // Use the custom claim type for user ID
                new Claim(ClaimTypes.Email, authenticatedUser.Email),
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}