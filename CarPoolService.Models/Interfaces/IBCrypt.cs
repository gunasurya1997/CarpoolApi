namespace CarPoolService.Models.Interfaces
{
    public interface IBCrypt
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
