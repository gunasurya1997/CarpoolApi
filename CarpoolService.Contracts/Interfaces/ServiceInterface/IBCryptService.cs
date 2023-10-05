namespace CarpoolService.Contracts.Interfaces.ServiceInterface
{
    public interface IBCryptService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
