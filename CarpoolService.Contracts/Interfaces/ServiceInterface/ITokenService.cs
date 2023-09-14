using CarpoolService.Contracts;

namespace CarPoolService.Contracts.Interfaces.Service_Interface
{
    public interface ITokenService
    {
        string GenerateToken(string issuer, string audience, string key, UserDTO authenticatedUser);
    }
}
