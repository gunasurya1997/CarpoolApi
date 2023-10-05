﻿using DTO = CarpoolService.Contracts.DTOs;

namespace CarpoolService.Contracts.Interfaces.ServiceInterface
{
    public interface ITokenService
    {
        string GenerateToken(string issuer, string audience, string key, DTO.User authenticatedUser);
    }
}
