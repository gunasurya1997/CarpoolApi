using CarpoolService.Contracts;
using CarPoolService.Models.DBModels;
namespace CarPoolService.Models.Interfaces.Service_Interface
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(User user);
        Task<UserDto> UpdateUserAsync(int userId, User updatedUser);
        Task<UserDto> AuthenticateUserAsync(Login loginUser);
        Task<UserDto> GetUserByIdAsync(int userId);

        Task<bool> IsEmailTakenAsync(string email);
    }
}
