using CarpoolService.Contracts;

namespace CarPoolService.Models.Interfaces.Service_Interface
{
    public interface ITokenService
    {
        string GenerateToken(string issuer, string audience, string key, UserDto authenticatedUser);
    }
}
